using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Prescription : Entity
    {
        [Key]
        public int PrescriptionId { get; set; }
        public Consultation Consultation { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string  Instructions { get; set; }
        public DateTime PrescribedDate { get; set; } = DateTime.Now;
    }
}
