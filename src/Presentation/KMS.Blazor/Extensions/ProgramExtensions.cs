using Domain.Aggregates.UserManagement.ValueTypes;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Blazor.Extensions;

static class ProgramExtensions
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(Policy.Lieutenant), policy => policy.RequireRole(Policy.Lieutenant.Roles));
            options.AddPolicy(nameof(Policy.Commander), policy => policy.RequireRole(Policy.Commander.Roles));
            options.AddPolicy(nameof(Policy.Observer), policy => policy.RequireRole(Policy.Observer.Roles));
        });

        return builder;
    }

    public static void AddOption<T>(this WebApplicationBuilder builder) where T : class
    {
        var section = builder.Configuration.GetSection(typeof(T).Name);
        builder.Services.Configure<T>(section);
    }
}