namespace MedicalUnitSystem.DTOs.Responses
{
    public class CreateAdmissionResponseDto
    {
        public int PatientId { get; set; }
        public DateTimeOffset AdmissionTime { get; set; }
        public bool IsDischarged { get; set; }
    }
}
