using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class PatientRequestDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string ContactInfo { get; set; }

        public string MedicalHistory { get; set; }
    }
}
