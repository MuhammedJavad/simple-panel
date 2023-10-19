using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Common.Validators;

public class IsHexAttribute : ValidationAttribute
{ 
    public override bool IsValid(object? value)
    {
        if (value is not string val) return false;
        var match = Regex.Match(val, "^([a-fA-F0-9]{2}\\s+)+");
        return match.Success;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} has invalid HEX format";
    }
}