using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class CreateConsultationRequestDto
    {
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public List<CreatePrescriptionRequestDto> Prescriptions { get; set; }
        public DateTime FollowupDate { get; set; }
    }
}
