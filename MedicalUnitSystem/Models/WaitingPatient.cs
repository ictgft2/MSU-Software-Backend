using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class WaitingPatient
    {
        [Key]
        public int WaitPatientId { get; set; }
        public bool AttendedTo { get; set; }
        public DateTime DateQueued { get; set; }
    }
}
