using System.ComponentModel.DataAnnotations;

namespace MedicalUnitSystem.Models
{
    public class LaboratoryTestType : Entity
    {
        [Key]
        public int LaboratoryTestTypeId { get; set; }
        public string LaboratoryTestTypeName { get; set; }

    }
}
