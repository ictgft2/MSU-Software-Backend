namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdateConsultationRequestDto
    {
        public DateTime ConsultationDate { get; set; } = DateTime.Now;
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public List<CreatePrescriptionRequestDto> Prescriptions { get; set; }
        public DateTime FollowupDate { get; set; }
    }
}
