using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class LaboratoryTestType : Entity
    {
        [Key]
        public int LaboratorytestTypeId { get; set; }
        public string LaboratoryTestName { get; set; }

    }
}
