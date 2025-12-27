using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISI_TP2_10444_SmartHealth_Data
{
    public enum AlertStatus
    {
        New = 0,
        Active = 1,
        Resolved = 2
    }
    [Table("Alerts")] // Defines the table name in the database
    public class Alert
    {
        [Key] // Primary Key Unic Identifier
        public Guid AlertId { get; private set; } // Verificar se para a base de dados deve ser criada PacientID e pacientID

        [ForeignKey("Patient")]// Foreign Key link to Patient
        public Guid PatientId { get; private set; }

        [Required(ErrorMessage = "O campo Regra violada é obrigatório.")]
        [MaxLength(250, ErrorMessage = "A descrição não pode exceder os 250 caracteres.")]
        public string RuleViolated { get; set; } // Ex: "Taquicardia > 120"
        public DateTime CreatedAt { get; private set; } = DateTime.Now; // If date is not provided, set one in UTC
        public AlertStatus Status { get; set; } = AlertStatus.New; // Default value is 'New'

        public virtual Patient Patient { get; set; } // Navigation Property: Allows accessing the Patient object directly
    }
}

