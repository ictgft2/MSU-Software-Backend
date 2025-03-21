namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetLaboratoryTestResponseDto
    {
        public int LaboratoryTestId { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public int PatientId { get; set; }
    }
}
