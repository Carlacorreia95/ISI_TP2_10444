using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISI_TP2_10444_SmartHealth_Data;
using ISI_TP2_10444_SmartHealth_Data.Dtos;
using ISI_TP2_10444_SmartHealth_Data.Services;

namespace ISI_TP2_10444_SmartHealth_IoTGateway.Controllers
{
    [ApiController]
    [Route("api/signals")]
    [AllowAnonymous]
    public class SignalController : ControllerBase
    {
        private readonly SmartHealthContext _db;
        private readonly ISoapRulesClient _soap;

        public SignalController(SmartHealthContext db, ISoapRulesClient soap)
        {
            _db = db;
            _soap = soap;
        }

        [HttpPost]
        public async Task<ActionResult<SignalsResponseDto>> PostSinais([FromBody] SignalsInputDto input)
        {
            var wearable = await _db.Wearables.FindAsync(input.WearableId);
            if (wearable is null)
                return NotFound("WearableID invalido.");

            var registoOrigemId = Guid.NewGuid();

            foreach (var m in input.Measurement)
            {
                _db.VitalSignRecords.Add(new VitalSignRecord
                {
                    RecordId = Guid.NewGuid(),
                    WearableId = input.WearableId,
                    RegistrationDateTime = input.RegistrationDateTime,
                    SignalType = m.SignalType,
                    Value = m.Value,
                    Unit = m.Unit
                });
            }

            await _db.SaveChangesAsync();

            var soapResult = await _soap.ProcessSignsAsync(new SoapRegistrationRequest
            {
                RecordId = registoOrigemId,
                PacientId = wearable.PatientId,
                RegistrationDateTime = input.RegistrationDateTime,
                Measurements = input.Measurement.Select(x => new SoapMeasurement
                {
                    SignalType = x.SignalType,
                    MeasurementValue = x.Value,
                    Unit = x.Unit
                }).ToList()
            });

            if (soapResult.AlertGenerated && soapResult.RuleViolated is not null)
            {
                _db.Alerts.Add(new Alert
                {
                    AlertId = Guid.NewGuid(),
                    PatientId = wearable.PatientId,
                    RuleViolated = soapResult.RuleViolated,
                    CreatedAt = DateTime.UtcNow,
                    Status = AlertStatus.New
                });

                await _db.SaveChangesAsync();
            }

            return Accepted(new SignalsResponseDto(
                Success: soapResult.Success,
                AlertGenerated: soapResult.AlertGenerated,
                Message: soapResult.Message ?? "OK"
            ));
        }

        [HttpGet("{patientId:guid}")]
        public async Task<IActionResult> GetHistory(Guid patientId)
        {
            var wearableIds = await _db.Wearables
                .Where(w => w.PatientId == patientId)
                .Select(w => w.WearableId)
                .ToListAsync();

            var result = await _db.VitalSignRecords
                .Where(v => wearableIds.Contains(v.WearableId))
                .OrderByDescending(v => v.RegistrationDateTime)
                .Take(200)
                .ToListAsync();

            return Ok(result);
        }
    }
}