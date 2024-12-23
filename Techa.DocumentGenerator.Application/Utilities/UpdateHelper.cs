using System;
using System.Linq.Expressions;
using System.Reflection;

public static class UpdateHelper
{
    public static void UpdateFields<T, TDto>(T target, TDto source, params Expression<Func<T, object?>>[] propertySelectors)
    {
        foreach (var propertySelector in propertySelectors)
        {
            // Extract property information from the lambda expression
            var memberExpression = propertySelector.Body as MemberExpression ??
                                   ((UnaryExpression)propertySelector.Body).Operand as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("Invalid property selector");

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Selector does not refer to a property");

            // Get the corresponding property in the source object
            var sourceProperty = typeof(TDto).GetProperty(property.Name);

            // Check if the target property exists in the target object
            var targetProperty = typeof(T).GetProperty(property.Name);

            if (sourceProperty != null && targetProperty != null)
            {
                // Get the value of the property from the source object
                var sourceValue = sourceProperty.GetValue(source);

                // If the source value is null, skip updating the target property
                if (sourceValue == null)
                {
                    continue; // Skip the update
                }

                // Convert and set the value if the source value is not null
                var targetValue = ConvertValue(sourceValue, targetProperty.PropertyType);
                targetProperty.SetValue(target, targetValue);
            }
        }
    }

    private static object? ConvertValue(object sourceValue, Type targetType)
    {
        var sourceType = sourceValue.GetType();

        if (targetType.IsAssignableFrom(sourceType))
        {
            return sourceValue;
        }

        if (IsNullableType(targetType))
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                return Convert.ChangeType(sourceValue, underlyingType);
            }
        }

        throw new ArgumentException($"Cannot assign value of type '{sourceType.FullName}' to property of type '{targetType.FullName}'");
    }

    private static bool IsNullableType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
