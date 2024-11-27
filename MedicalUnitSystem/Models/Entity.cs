namespace MedicalUnitSystem.Models
{
    public class Entity
    {
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
