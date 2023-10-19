using Domain.Common.Constants;

namespace Domain.Common.Types;

public class DomainException : Exception
{
    public ExceptionCode Code { get; }

    public DomainException(ExceptionCode code)
    {
        Code = code;
    }
}