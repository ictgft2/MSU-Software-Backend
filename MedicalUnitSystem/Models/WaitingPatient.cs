using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class WaitingPatient : Entity
    {
        [Key]
        public int WaitingPatientId { get; set; }
        public bool AttendedTo { get; set; }
        public DateTime DateQueued { get; set; }
        public Patient Patient { get; set; }
    }
}
