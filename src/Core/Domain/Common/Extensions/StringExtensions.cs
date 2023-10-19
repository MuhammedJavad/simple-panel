using System.Security;

namespace Domain.Common.Extensions;

public static class StringExtensions
{
    public static SecureString ToSecureString(this string? value)
    {
        var ss = new SecureString();
        if (string.IsNullOrWhiteSpace(value)) return ss;
        foreach (var ch in value.ToCharArray()) ss.AppendChar(ch);
        return ss;
    }

    public static bool IsEmpty(this string? s)
    {
        return string.IsNullOrWhiteSpace(s);
    }
    
    public static bool CultureIgnoreEqual(this string s, string to)
    {
        return string.Equals(s, to, StringComparison.InvariantCultureIgnoreCase);
    }
}