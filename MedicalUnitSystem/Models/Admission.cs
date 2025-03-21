using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Admission : Entity
    {
        [Key]
        public int AdmissionId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTimeOffset AdmissionTime { get; set; }
        public bool IsDischarged { get; set; }
    }
}
