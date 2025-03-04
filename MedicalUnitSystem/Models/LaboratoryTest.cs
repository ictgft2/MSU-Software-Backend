using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class LaboratoryTest : Entity
    {
        [Key]
        public int LaboratoryTestId { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public LaboratoryTestType LaboratoryTestType { get; set; }

    }
}
