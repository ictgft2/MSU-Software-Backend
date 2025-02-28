using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class LaboratoryTest
    {
        [Key]
        public int LaboratoryTestId { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public Patient Patient { get; set; }
        public LaboratoryTestType LaboratoryTestType { get; set; }

    }
}
