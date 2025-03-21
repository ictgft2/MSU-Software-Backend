namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetPrescriptionResponseDto
    {
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
    }
}
