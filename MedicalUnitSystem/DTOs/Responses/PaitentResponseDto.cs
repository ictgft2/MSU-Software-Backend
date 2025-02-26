namespace MedicalUnitSystem.DTOs.Responses
{
    public class PaitentResponseDto
    {
        public int PatientId { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string ContactInfo { get; set; }

        public string MedicalHistory { get; set; }
    }
}
