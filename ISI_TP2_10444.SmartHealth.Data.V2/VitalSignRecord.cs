using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI_TP2_10444_SmartHealth_Data
{
    public class VitalSignRecord
    {
        public Guid RecordId { get; set; }
        public Guid WearableId { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string SignalType { get; set; } = "";
        public decimal Value { get; set; }
        public string Unit { get; set; } = "";
    }
}
