using MedicalUnitSystem.DTOs.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class CreatePatientRequestDto
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public GenderEnum GenderId { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; } = null;

        public string MedicalHistory { get; set; }
    }
}
