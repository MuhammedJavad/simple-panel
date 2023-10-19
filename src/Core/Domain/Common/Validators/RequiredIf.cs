using System.ComponentModel.DataAnnotations;

namespace Domain.Common.Validators;

using System;
using System.ComponentModel.DataAnnotations;

public class RequiredIf : ValidationAttribute
{
    private string ConditionalPropertyName { get; }
    private object TargetValue { get; }

    public RequiredIf(string conditionalPropertyName, object targetValue)
    {
        ConditionalPropertyName = conditionalPropertyName;
        TargetValue = targetValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var conditionalPropertyValue = validationContext.ObjectType
            .GetProperty(ConditionalPropertyName)?
            .GetValue(validationContext.ObjectInstance);
        
        if (conditionalPropertyValue == null || !conditionalPropertyValue.Equals(TargetValue))
            return ValidationResult.Success;
        
        return value == default ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
}