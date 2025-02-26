using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Vital
    {
        [Key]
        public Guid Id { get; set; }
        public string  BloodPressure { get; set; }
        public DateTime DateOfVisit { get; set; } = DateTime.UtcNow;
        public Patient Patient { get; set; }
    }
}
