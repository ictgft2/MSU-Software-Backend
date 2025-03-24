namespace MedicalUnitSystem.DTOs.Responses
{
    public class DischargePatientResponseDto
    {
        public string PatientId { get; set; }
        public string DischargeNotes { get; set; }
        public bool IsDischarged { get; set; }
    }
}
