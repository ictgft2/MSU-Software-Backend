using MedicalUnitSystem.DTOs.Requests;

namespace MedicalUnitSystem.DTOs.Responses
{
    public class CreateConsultationResponseDto
    {
        public int ConsultationId { get; set; }
        public DateTime ConsultationDate { get; set; } = DateTime.Now;
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public List<CreatePrescriptionRequestDto> Prescriptions { get; set; }
        public DateTime FollowupDate { get; set; }
    }
}
