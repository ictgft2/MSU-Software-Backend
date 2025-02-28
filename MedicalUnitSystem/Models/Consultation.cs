using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Consultation
    {
        [Key]
        public int ConsultationId { get; set; }
        public string BloodPressure { get; set; }
        public string Diagnosis { get; set; }
        public string Prescriptions { get; set; }
        public string LaboratoryTests { get; set; }
        public Patient Patient { get; set; }
    }
}
