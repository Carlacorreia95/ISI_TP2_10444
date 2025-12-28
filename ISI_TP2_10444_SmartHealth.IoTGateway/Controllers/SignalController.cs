using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ISI_TP2_10444_SmartHealth_Data;
using Microsoft.EntityFrameworkCore;
using ISI_TP2_10444_SmartHealth_Data.Dtos;
using ISI_TP2_10444_SmartHealth_SoapRules.Services;
using ISI_TP2_10444_SmartHealth_Data.Services;

namespace SmartHealth.Api.Controllers;

[ApiController]
[Route("api/signals")]
[Authorize]
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
        // 1) validar wearable
        var wearable = await _db.Wearables.FindAsync(input.WearableId);
        if (wearable is null)
            return NotFound("WearableID inválido.");

        // 2) persistir medições (1 linha por medição)
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

        // 3) chamar motor de regras (SOAP stub por enquanto)
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

        // 4) se gerou alerta, guardar
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

        // 5) responder ao wearable
        return Accepted(new SignalsResponseDto(
            Success: soapResult.Success,
            AlertGenerated: soapResult.AlertGenerated,
            Message: soapResult.Message ?? "OK"
        ));
    }

    [HttpGet("{patientId:guid}")]
    public async Task<IActionResult> GetHistory(Guid patientId)
    {
        // wearables of patient
        var wearableIds = await _db.Wearables
            .Where(w => w.PatientId == patientId)
            .Select(w => w.WearableId)
            .ToListAsync();

        // signs of these wearables
        var result = await _db.VitalSignRecords
            .Where(v => wearableIds.Contains(v.WearableId))
            .OrderByDescending(v => v.RegistrationDateTime)
            .Take(200)
            .ToListAsync();

        return Ok(result);
    }
}
