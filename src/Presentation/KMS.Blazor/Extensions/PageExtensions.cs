using Domain.Common.Types;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blazor.Extensions;

public static class PageExtensions
{
    public static void AddToModelStateError(this R r, ModelStateDictionary modelState)
    {
        if (r.IsOk) 
            throw new ApplicationException($"Can not add {nameof(R)}.{nameof(R.ErrorMessage)} to {nameof(ModelStateDictionary)}, because {nameof(R)}.{nameof(R.IsOk)} is true");
        modelState.TryAddModelError("", r.ErrorMessage!);
    }
}