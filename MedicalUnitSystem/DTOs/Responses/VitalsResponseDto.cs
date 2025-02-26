namespace MedicalUnitSystem.DTOs.Responses
{
    public class VitalsResponseDto
    {
        public int PatientId { get; set; }
        public string BloodPressure{ get; set; }
        public DateTime DateOfVisit { get; set; }
    }
}
