using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ISI_TP2_10444_SmartHealth_Data
{
    [cite_start]
    internal class Wearable
    {
        [Key]
        public Guid WearableID { get; set; }
        public Guid PatientID { get; set; } // FK
        public string SensorType { get; set; } // Ex: "Apple Watch"
        public int BatteryStatus { get; set; }

        [ForeignKey("PatientID")]
        public Patient Patient { get; set; }
    }
}
