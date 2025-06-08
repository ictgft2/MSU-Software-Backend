using MedicalUnitSystem.Helpers.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace MedicalUnitSystem.Helpers
{
    public static class Extensions
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, PropertyInfo property, SortOrder order)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property.Name);
            var propertyType = property.PropertyType;

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propertyType);
            var lambda = Expression.Lambda(delegateType, propertyAccess, parameter);

            var methodName = order == SortOrder.Descending ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable)
                .GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), propertyType);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda });
        }
    }
}
