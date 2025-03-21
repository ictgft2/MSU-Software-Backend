namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetConsultationResponseDto
    {
        public int ConsultationId { get; set; }
        public DateTime ConsultationDate { get; set; } 
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public List<GetPrescriptionResponseDto> Prescriptions { get; set; }
        public DateTime FollowupDate { get; set; }
    }
}
