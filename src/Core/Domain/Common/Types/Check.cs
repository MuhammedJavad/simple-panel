using Domain.Common.Constants;

namespace Domain.Common.Types;

public class Check
{
    public static void True(bool condition, ExceptionCode code)
    {
        if (!condition) throw new DomainException(code);
    }
    
    public static void NotEmpty(string value, ExceptionCode code)
    {
        // todo; check property names
        if (string.IsNullOrWhiteSpace(value)) throw new DomainException(code);
    }
    
    public static void If(bool condition, ExceptionCode code)
    {
        if (condition) throw new DomainException(code);
    }
}