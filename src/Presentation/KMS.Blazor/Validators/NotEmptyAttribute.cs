using System.ComponentModel.DataAnnotations;

namespace Blazor.Validators;

public class NotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        var type = value.GetType();
        if (!type.IsValueType) return true;
        var defaultValue = Activator.CreateInstance(type);
        return !value.Equals(defaultValue);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} can not be empty";
    }
}