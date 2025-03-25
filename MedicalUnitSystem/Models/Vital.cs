using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalUnitSystem.Models
{
    public class Vital : Entity
    {
        [Key]
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VitalsId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateOfVisit { get; set; } = DateTime.UtcNow;
        [Required]
        [Range(30, 200)] // Normal range for BPM
        public int HeartRate { get; set; } // Beats per minute
        [Required]
        [Range(70, 200)] // Systolic range
        public int SystolicBloodPressure { get; set; } // mmHg
        [Required]
        [Range(40, 120)] // Diastolic range
        public int DiastolicBloodPressure { get; set; } // mmHg
        [Required]
        [Range(30.0, 45.0)] // Body temperature in Celsius
        public double Temperature { get; set; } // Celsius
        [Required]
        [Range(8, 40)] // Breaths per minute
        public int RespiratoryRate { get; set; }
        [Required]
        [Range(80, 100)] // Normal oxygen saturation
        public int OxygenSaturation { get; set; } // SpO2 percentage
        public string Notes { get; set; } // Optional field for additional observations
        public Patient Patient { get; set; }
    }
}
