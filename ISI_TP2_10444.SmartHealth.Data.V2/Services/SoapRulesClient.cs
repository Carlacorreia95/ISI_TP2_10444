using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI_TP2_10444_SmartHealth_Data.Services
{
    public interface ISoapRulesClient
    {
        Task<SoapResult> ProcessSignsAsync(SoapRegistrationRequest req);
    }

    public class SoapRulesClient : ISoapRulesClient
    {
        public Task<SoapResult> ProcessSignsAsync(SoapRegistrationRequest req)
        {
            var fever = req.Measurements.Any(m => m.SignalType == "Temp" && m.MeasurementValue >= 38.5m);
            var taq = req.Measurements.Any(m => m.SignalType == "FC" && m.MeasurementValue >= 100m);

            if (fever)
                return Task.FromResult(new SoapResult(true, true, "Alerta Febre criado", "Febre"));

            if (taq)
                return Task.FromResult(new SoapResult(true, true, "Alerta Taquicardia criado", "Taquicardia"));

            return Task.FromResult(new SoapResult(true, false, "Nenhum alerta crítico detectado", null));
        }
    }

    public class SoapRegistrationRequest
    {
        public Guid RecordId { get; set; }
        public Guid PacientId { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public List<SoapMeasurement> Measurements { get; set; } = new();
    }

    public class SoapMeasurement
    {
        public string SignalType { get; set; } = "";
        public decimal MeasurementValue { get; set; }
        public string Unit { get; set; } = "";
    }

    public record SoapResult(bool Success, bool AlertGenerated, string? Message, string? RuleViolated);
}
