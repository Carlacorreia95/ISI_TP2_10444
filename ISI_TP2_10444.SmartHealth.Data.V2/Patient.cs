using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISI_TP2_10444_SmartHealth_Data
{
    ///
    [Table("Patients")] // define the name of table
    public class Patient
    {
        [Key] // Primary Key Unic Identifier
        public Guid PatientId { get; set; } 
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O nome não pode exceder os 150 caracteres.")]
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(200)]
        public string ChronicCondition { get; set; } // Ex: "Diabetes"
        public int? DoctorId { get; set; } // Chave estrangeira

        public string UserType { get; set; } // Ex: "Patient"

        public string UserName { get; set; } // Ex: "johndoe"

        public string Password { get; set; } // Ex: "hashed_password"   

        // --- Navigation Properties ---

        //public ICollection<Alert> Alerts { get; set; }
        //public virtual ICollection<Wearable> Wearables { get; set; } // Navigation Property: Allows accessing related Wearables directly

    }
}
