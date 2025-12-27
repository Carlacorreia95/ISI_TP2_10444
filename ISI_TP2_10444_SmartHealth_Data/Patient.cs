using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISI_TP2_10444_SmartHealth_Data
{
    [Table("Patients")] // define the name of table
    public class Patient
    {
        [Key] // Primary Key Unic Identifier
        public Guid PatientID { get; private set; } // Verificar se para a base de dados deve ser criada PacientID e pacientID
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O nome não pode exceder os 150 caracteres.")]
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(200)]
        public string ChronicCondition { get; set; } // Ex: "Diabetes"
        public int? DoctorID { get; set; } // Chave estrangeira
    }
}
