using System.Text.Json.Serialization;
using Domain.Common.Constants;
using Humanizer;

namespace Domain.Common.Types;

/// <summary>
/// 
/// R stands for Result
/// </summary>
public record R
{
    public static readonly R None = new(false);
    private readonly ExceptionCode _code;
    private readonly string _errorMessage;
    private readonly bool _isOk;
    public R() : this(true) { }
    public R(bool isOk, ExceptionCode code = Failed)
    {
        _isOk = isOk;
        _code = code;
        _errorMessage = code.Humanize();
    }
    public R(bool isOk, string errorMessage)
    {
        _isOk = isOk;
        _code = default;
        _errorMessage = errorMessage;
    }
    [JsonIgnore] public bool IsOk => _isOk;
    [JsonIgnore] public bool IsNotOk => !_isOk;
    public string ErrorMessage => _errorMessage;
    public ExceptionCode MessageCode() => _code;
    public string Message() => _code.Humanize();
    public R Map(Func<R> @continue) => IsOk ? @continue.Invoke() : MessageCode();
    public R<TS> Map<TS>(Func<R<TS>> @continue) => IsOk ? @continue.Invoke() : MessageCode();
    public static implicit operator R(bool a) => a ? new R() : None;
    public static implicit operator R(ExceptionCode a) => new(false, a);
}

public record R<T> : R
{
    public R(bool wasSuccessful, T result) : base(wasSuccessful)
    {
        Result = result;
    }

    public R(T result) : base(true)
    {
        Result = result;
    }

    public R(ExceptionCode code = Failed) : base(false, code) { }
    public T? Result { get; }
    public R Map(Func<T, R> @continue) => IsOk ? @continue.Invoke(Result!) : MessageCode();
    public R<TS> Map<TS>(Func<T, R<TS>> @continue) => IsOk ? @continue.Invoke(Result!) : MessageCode();
    public ValueTask<R<TS>> Map<TS>(Func<T, Task<R<TS>>> @continue) => MapInternal(@continue);
    public ValueTask<R> Map(Func<T, Task<R>> @continue) => MapInternal(@continue);
    public void EnsureOk()
    {
        if (IsNotOk)
        {
            throw new DomainException(MessageCode());
        }
    }
    private async ValueTask<TR> MapInternal<TR>(Func<T, Task<TR>> @continue) where TR : R
    {
        if (IsOk)
        {
            return await @continue.Invoke(Result!);
        }

        return (TR)MessageCode();
    }
    public static implicit operator R<T>(T input) => input != null ? new R<T>(input) : new R<T>();
    public static implicit operator R<T>(ExceptionCode input) => new(input);
}