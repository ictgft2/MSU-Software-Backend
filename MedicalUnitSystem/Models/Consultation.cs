using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class Consultation : Entity
    {
        [Key]
        public int ConsultationId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ConsultationDate { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public DateTime FollowupDate { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
