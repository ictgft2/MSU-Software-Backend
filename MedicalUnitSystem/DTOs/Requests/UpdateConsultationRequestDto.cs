namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdateConsultationRequestDto
    {
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public List<UpdatePrescriptionRequestDto> Prescriptions { get; set; }
        public DateTime FollowupDate { get; set; }
    }
}
