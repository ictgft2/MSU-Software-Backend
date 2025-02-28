using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class ConsultationRequestDto
    {
        public string BloodPressure { get; set; }
        public string Diagnosis { get; set; }
        public string Prescriptions { get; set; }
        public string LaboratoryTests { get; set; }
    }
}
