using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Waitlist
    {
        [Key]
        public int WaitlistId { get; set; }
        public DateTime DateQueued { get; set; }
    }
}
