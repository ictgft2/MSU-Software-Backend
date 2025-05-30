﻿using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Prescription : Entity
    {
        [Key]
        public int PrescriptionId { get; set; }
        public int ConsultationId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string  Instructions { get; set; }
        public bool IsDispensed { get; set; } = false;
        public Consultation Consultation { get; set; }
        public DateTimeOffset PrescribedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
