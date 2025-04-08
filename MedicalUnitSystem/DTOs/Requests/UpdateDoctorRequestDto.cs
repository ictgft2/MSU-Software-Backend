using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdateDoctorRequestDto
    {
        public string Name { get; set; }

        public int GenderId { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; } = null;
    }
}
