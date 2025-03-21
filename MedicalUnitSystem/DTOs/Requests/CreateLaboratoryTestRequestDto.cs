namespace MedicalUnitSystem.DTOs.Requests
{
    public class CreateLaboratoryTestRequestDto
    {
        public int LaboratoryTestTypeId { get; set; }
        public int PatientId { get; set; }
    }
}
