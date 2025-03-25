using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdateVitalsRequestDto
    {
        [Required]
        [Range(30, 200)]
        public int HeartRate { get; set; }
        [Required]
        [Range(70, 200)]
        public int SystolicBloodPressure { get; set; }
        [Required]
        [Range(40, 120)]
        public int DiastolicBloodPressure { get; set; }
        [Required]
        [Range(30.0, 45.0)]
        public double Temperature { get; set; }
        [Required]
        [Range(8, 40)]
        public int RespiratoryRate { get; set; }
        [Required]
        [Range(80, 100)]
        public int OxygenSaturation { get; set; }
        public string Notes { get; set; }
    }
}
