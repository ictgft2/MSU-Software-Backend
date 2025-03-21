using System.Reflection;

namespace MedicalUnitSystem.Helpers
{
    public interface IPropertyCheckingService
    {
        PropertyInfo CheckProperty<T>(string propertyName);
    }
}
