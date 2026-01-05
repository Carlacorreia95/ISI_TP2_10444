using CoreWCF;
using System.Runtime.Serialization;

namespace ISI_TP2_10444_SmartHealth_SoapRules.Contracts;

[ServiceContract]
public interface IRulesService
{
    [OperationContract]
    RulesResponse SignalProcessing(RulesRequest request);
}

[DataContract]
public class RulesRequest
{
    [DataMember] public Guid RecordId { get; set; }
    [DataMember] public Guid PatientId { get; set; }
    [DataMember] public DateTime RecordDateTime { get; set; }
    [DataMember] public List<MeasurementRules> Mesurements { get; set; } = new();
}

[DataContract]
public class MeasurementRules
{
    [DataMember] public string SignalType { get; set; } = "";
    [DataMember] public decimal SignalValue { get; set; }
    [DataMember] public string Unit { get; set; } = "";
}

[DataContract]
public class RulesResponse
{
    [DataMember] public bool Success { get; set; }
    [DataMember] public bool AlertGenerated { get; set; }
    [DataMember] public string Message { get; set; } = "";
    [DataMember] public string? RuleViolated { get; set; }
}
