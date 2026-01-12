using System.ComponentModel.DataAnnotations.Schema;

namespace ISI_TP2_10444_SmartHealth_Data
{
    [Table("Wearables")]
    public class Wearable
    {
        public Guid WearableId { get; set; }
        public Guid PatientId { get; set; }
        public string SensorType { get; set; } = "";
        public string Model { get; set; } = "";
        public int BateryState { get; set; }
        public DateTime LastSync { get; set; }
    }
}