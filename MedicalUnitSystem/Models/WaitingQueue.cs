using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class WaitingQueue : Entity
    {
        [Key]
        public int WaitingQueueId { get; set; }
        public int PatientId { get; set; }
        public bool AttendedTo { get; set; }
        public DateTime DateQueued { get; set; } = DateTime.UtcNow;
        public Patient Patient { get; set; }
    }
}
