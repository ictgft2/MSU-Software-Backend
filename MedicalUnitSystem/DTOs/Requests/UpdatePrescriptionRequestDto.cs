namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdatePrescriptionRequestDto
    {
        public int PrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
    }
}
