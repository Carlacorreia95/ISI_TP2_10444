using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI_TP2_10444_SmartHealth_Data
{
    internal class RegisterSignal
    {
        [Key]
        public Guid RegisteredID { get; set; }
        public Guid WearableID { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string SignalType { get; set; } // "FC" (Frequência Cardíaca), "Temp"
        public double Value { get; set; }
        public string Unit { get; set; }
    }
}
