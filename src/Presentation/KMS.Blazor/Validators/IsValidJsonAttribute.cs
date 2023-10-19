using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Blazor.Validators;

class IsValidJsonAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string jsonStr) return false;
        if (string.IsNullOrWhiteSpace(jsonStr)) return true;
        try
        {
            JsonNode.Parse(jsonStr);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} has invalid JSON format";
    }
}