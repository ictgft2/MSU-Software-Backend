﻿using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Doctor : Entity
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public string Name { get; set; }
        public int GenderId { get; set; }

        public string Email { get; set; } = null;
        public string Phone { get; set; } = null;
        public Gender Gender { get; set; }
        public ICollection<Consultation> Consultations { get; set; }
    }
}
