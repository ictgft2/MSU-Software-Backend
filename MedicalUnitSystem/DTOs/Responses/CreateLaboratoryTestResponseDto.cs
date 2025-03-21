namespace MedicalUnitSystem.DTOs.Responses
{
    public class CreateLaboratoryTestResponseDto
    {
        public int LaboratoryTestTypeId { get; set; }
        public int PatientId { get; set; }
        public int LaboratoryTestId { get; set; }
    }
}
