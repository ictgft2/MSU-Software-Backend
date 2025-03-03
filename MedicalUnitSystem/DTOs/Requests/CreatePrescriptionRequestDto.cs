namespace MedicalUnitSystem.DTOs.Requests
{
    public class CreatePrescriptionRequestDto
    {
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
    }
}
