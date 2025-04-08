namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdatePatientRequestDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public int GenderId { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; } = null;
    }
}
