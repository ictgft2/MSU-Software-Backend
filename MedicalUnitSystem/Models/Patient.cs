using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
	public class Patient : Entity
	{
        [Key]
        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; }
        public int GenderId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PatientNumber { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; } = null;
        public string Phone { get; set; } = null;
        public string MedicalHistory { get; set; }
        public ICollection<Consultation> Consultations { get; set; }
        public ICollection<LaboratoryTest> LaboratoryTests { get; set; }
        public ICollection<Vital> Vitals { get; set; }
        public ICollection<WaitingQueue> Waitlists { get; set; }
    }
}

