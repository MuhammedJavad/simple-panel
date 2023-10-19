using Domain.Common.Constants;
using Domain.Common.Types;

namespace Domain.Common.Extensions;

public static class RExtensions
{
    public static R<T> ToResult<T>(this T input, ExceptionCode code = Failed)
    {
        return input != null ? new R<T>(input) : new R<T>(code);
    }

    public static R<T> ToResult<T>(this T input, Func<T, bool> predict, ExceptionCode code = Failed)
    {
        var result = predict.Invoke(input);
        return result ? new R<T>(input) : new R<T>(code);
    }

    // public static async Task<R> SafeExecutor<T>(
    //     this IServiceProvider serviceProvider, 
    //     Func<T, Task<R>> func)  where T : notnull
    // {
    //     try
    //     {
    //         var service = serviceProvider.GetRequiredService<T>();
    //         return await func.Invoke(service);
    //     }
    //     catch (DomainException e)
    //     {
    //         return e.Code;
    //     }
    //     catch (Exception e)
    //     {
    //         
    //         return failureExCode;
    //     }
    // }
}