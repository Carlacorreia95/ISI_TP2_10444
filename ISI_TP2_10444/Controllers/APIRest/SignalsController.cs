using ISI_TP2_10444.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ISI_TP2_10444.Services.Soap;

namespace ISI_TP2_10444.Controllers.APIRest
{
    // Aplica o filtro de segurança (requer um token Bearer válido)
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SignalsController : ControllerBase
    {
        private readonly IServiceMotorRules _serviceMotorRules; // Interface para o Serviço SOAP

        public SignalsController(IServiceMotorRules serviceMotorRules)
        {
            _serviceMotorRules = serviceMotorRules;
        }

        /// <summary>
        /// Recebe dados de sinais vitais de um wearable simulado.
        /// </summary>
        /// <param name="input">Dados do sensor.</param>
        /// <returns>Resultado do processamento, incluindo se um alerta foi gerado.</returns>
        [HttpPost]
        [Route("sinais")]
        public async Task<IActionResult> PostSignals([FromBody] RegistVitalsSingals input)
        {
            if (input == null || input.Measurements == null || input.Measurements.Count == 0 ||!ModelState.IsValid)
            {
                return BadRequest("Dados de sinais vitais inválidos.");
            }

            var patientId = () =>
            {
                // Aqui implementar a lógica para obter o PatientID real
                // Por exemplo, a partir do token JWT ou de outro serviço

                return System.Guid.NewGuid(); // Retorna um GUID fictício por enquanto
            };
            var registId = input.Id != System.Guid.Empty ? input.Id : System.Guid.NewGuid();

            try
            {
                // Chama o serviço SOAP para processar os sinais vitais
                var resultProcessing = await _serviceMotorRules.ProcessingSignalsAsync(
                    input.Id,
                    patientId(), // Substitua por PatientID real se disponível
                    input.DateHour,
                    input.Measurements
                );

                if (resultProcessing.GeneratedAlerts)
                {
                    // Devolve 202 Accepted
                    return Accepted(resultProcessing);
                }
                return Ok(resultProcessing);
            }
            catch (System.Exception ex)
            {
                // Regista o erro (aqui apenas devolvemos o erro na resposta)
                return StatusCode(500, $"Erro ao processar os sinais vitais: {ex.Message}");
            }                        
        }
    }
}
