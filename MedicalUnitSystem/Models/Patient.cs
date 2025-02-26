using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
	public class Patient
	{
        [Key]
        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string ContactInfo { get; set; }

        public string MedicalHistory { get; set; }

        public ICollection<Consultation> Consultations { get; set; }
        public ICollection<LaboratoryTest> LaboratoryTests { get; set; }
        public ICollection<Vital> Vitals { get; set; }
        public ICollection<Waitlist> Waitlists { get; set; }
    }
}

