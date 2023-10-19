using System.Security.Claims;
using Domain.Common.Extensions;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Extensions;

public static class HttpContextExtensions
{
    public static bool TryGetCurrentUserId(this AuthenticationState? auth, out long id)
    {
        id = default;
        if (auth == null) return false;
        var userIdStr = auth.User.FindFirstValue(ClaimTypes.Sid);
        if (userIdStr.IsEmpty()) return false;
        return long.TryParse(userIdStr, out id) ;
    }
}