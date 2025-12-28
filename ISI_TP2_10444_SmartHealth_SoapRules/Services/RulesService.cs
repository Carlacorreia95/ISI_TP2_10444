using ISI_TP2_10444_SmartHealth_SoapRules.Contracts;

namespace ISI_TP2_10444_SmartHealth_SoapRules.Services;

public class RulesService : IRulesService
{
    public RulesResponse SignalProcessing(RulesRequest request)
    {
        var fever = request.Mesurements.Any(m => m.SignalType == "Temp" && m.SignalValue >= 38.5m);
        var taq = request.Mesurements.Any(m => m.SignalType == "FC" && m.SignalValue >= 100m);

        if (fever)
            return new RulesResponse { Success = true, AlertGenerated = true, RuleViolated = "Febre", Message = "Alerta Febre criado" };

        if (taq)
            return new RulesResponse { Success = true, AlertGenerated = true, RuleViolated = "Taquicardia", Message = "Alerta Taquicardia criado" };

        return new RulesResponse { Success = true, AlertGenerated = false, Message = "Nenhum alerta crítico detectado" };
    }
}
