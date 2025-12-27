using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISI_TP2_10444_SmartHealth_Data
{
    ///
    [Table("Patients")] // define the name of table
    public class Patient
    {
        [Key] // Primary Key Unic Identifier
        public Guid PatientId { get; set; } // Verificar se para a base de dados deve ser criada PacientID e pacientID
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O nome não pode exceder os 150 caracteres.")]
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(200)]
        public string ChronicCondition { get; set; } // Ex: "Diabetes"
        public int? DoctorId { get; set; } // Chave estrangeira

        // --- Navigation Properties ---

        public virtual ICollection<Alert> Alerts { get; set; } // Navigation Property: Allows accessing related Alerts directly
        //public virtual ICollection<Wearable> Wearables { get; set; } // Navigation Property: Allows accessing related Wearables directly

        public Patient()  //Inicializa as listas vazias para evitar erros de "NullReference" ao adicionar algo antes de carregar da BD.
        {
            Alerts = new List<Alert>();
            //Wearables = new List<Wearable>();
        }
    }
}
