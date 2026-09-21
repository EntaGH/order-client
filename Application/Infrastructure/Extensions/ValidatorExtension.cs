using AutoMapper.Internal;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Application.Infrastructure.Extensions;

public static class ValidatorExtension
{
    public static IRuleBuilderOptions<T, ICollection<TProperty>?> NotDuplicate<T, TProperty>(this IRuleBuilder<T, ICollection<TProperty>?> ruleBuilder)
    {
        return ruleBuilder.Must(collections => collections != null && collections.Distinct().Count() == collections.Count);
    }

    public static IRuleBuilderOptions<T, ICollection<TProperty>?> GreaterOrEqualTo<T, TProperty>(this IRuleBuilder<T, ICollection<TProperty>?> ruleBuilder, int value)
    {
        return ruleBuilder.Must(collections => collections == null || collections.Count >= value);
    }

    public static IRuleBuilderOptions<T, ICollection<TProperty>?> LessThanOrEqualTo<T, TProperty>(this IRuleBuilder<T, ICollection<TProperty>?> ruleBuilder, int value)
    {
        return ruleBuilder.Must(collections => collections == null || collections.Count <= value);
    }
}

public static class ValidationContextExtension
{
    public static IEnumerable<ValidationResult> Required(this ValidationContext validationContext, params string[] ignoreProperties)
    {
        foreach (PropertyInfo propertyInfo in validationContext.ObjectType.GetProperties())
        {
            if (ignoreProperties.Contains(propertyInfo.Name, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            Type propertyType = propertyInfo.PropertyType;
            object? propValue = propertyInfo.GetValue(validationContext.ObjectInstance);
            object? defaultVal;
            string message = $"{propertyInfo.Name} of {validationContext.ObjectType.FullName} is required";
            if (propertyType == typeof(string))
            {
                if (string.IsNullOrEmpty(propValue?.ToString()))
                {
                    yield return new ValidationResult(
                    message,
                    new[] { propertyInfo.Name });
                }
            }
            else
            {
                defaultVal = propertyType.IsNullableType() ? null : Activator.CreateInstance(propertyType);

                if (propValue?.Equals(defaultVal) == true)
                {
                    yield return new ValidationResult(
                        message,
                        new[] { propertyInfo.Name });
                }
            }
        }
    }
}