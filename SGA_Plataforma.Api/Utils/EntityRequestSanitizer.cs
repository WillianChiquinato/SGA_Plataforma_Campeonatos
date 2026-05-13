using System.Collections;
using System.Reflection;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Utils;

public static class EntityRequestSanitizer
{
    public static TEntity StripNavigations<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanWrite || !IsNavigationProperty(property.PropertyType))
            {
                continue;
            }

            property.SetValue(entity, null);
        }

        return entity;
    }

    public static bool IsNavigationProperty(PropertyInfo property)
    {
        return IsNavigationProperty(property.PropertyType);
    }

    private static bool IsNavigationProperty(System.Type propertyType)
    {
        var actualType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (typeof(BaseEntity).IsAssignableFrom(actualType))
        {
            return true;
        }

        if (actualType == typeof(string) || !typeof(IEnumerable).IsAssignableFrom(actualType))
        {
            return false;
        }

        if (!actualType.IsGenericType)
        {
            return false;
        }

        return actualType
            .GetGenericArguments()
            .Select(argument => Nullable.GetUnderlyingType(argument) ?? argument)
            .Any(typeof(BaseEntity).IsAssignableFrom);
    }
}