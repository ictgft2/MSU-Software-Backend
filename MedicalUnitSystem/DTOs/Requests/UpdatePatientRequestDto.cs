using MedicalUnitSystem.DTOs.Enums;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdatePatientRequestDto
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public GenderEnum GenderId { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; } = null;
    }
}
