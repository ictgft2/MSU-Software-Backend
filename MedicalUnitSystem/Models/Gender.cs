using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Gender : Entity
    {
        [Key]
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
