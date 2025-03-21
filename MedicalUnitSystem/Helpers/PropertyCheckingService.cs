using System.Reflection;

namespace MedicalUnitSystem.Helpers
{
    public class PropertyCheckingService : IPropertyCheckingService
    {
        public PropertyInfo CheckProperty<T>(string propertyName)
        {
            if(propertyName is not null)
            {
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance);

                if(propertyInfo == null)
                {
                    throw new ArgumentException("Property not found", nameof(propertyName));
                }
                
                return propertyInfo;
            }

            return null;
        }
    }
}
