using System;
using System.Collections.Generic;

namespace ISI_TP2_10444.Models
{
    // Classe que representa a medição individual do sensor
    public class Measurement
    {
        public string SignalType { get; set; } // Ex: "FC", "Temp"
        public decimal Value { get; set; }
        public string Unit { get; set; } // Ex: "bpm", "ºC"
    }

    // Classe que representa o registo de sinais vitais de um paciente
    public class RegistVitalsSingals
    {
        public Guid Id { get; set; } // Identificador único do registo
        public DateTime DateHour { get; set; } // Data e hora do registo
        public List<Measurement> Measurements { get; set; } // Lista de medições realizadas
        public RegistVitalsSingals()
        {
            Measurements = new List<Measurement>();
        }
    }

    // Classe que representa a resposta após o processamento
        public class ResultProcessing
    {
        public bool Success { get; set; }
        public bool GeneratedAlerts { get; set; }
        public string Message { get; set; }
        public Guid? NewAlertId { get; set; }
    }


}
