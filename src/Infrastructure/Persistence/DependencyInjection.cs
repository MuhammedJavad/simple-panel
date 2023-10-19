using Domain.Aggregates.UserManagement;
using Domain.Aggregates.VendorManagement;
using Domain.Aggregates.VendorManagement.Repositories;
using Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Domain.VendorManagement.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddSingleton<ISqlConnectionFactory>(p =>
        {
            var cs = p.GetRequiredConnectionString();
            return new SqlConnectionFactory(cs);
        });
        services.AddDbContext<AppContext>((p, builder) =>
        {
            var cs = p.GetRequiredConnectionString();
            builder.UseMySQL(cs);
        });
        services.AddScoped<IVendorContext, AppContext>();
        services.AddScoped<IUserContext, AppContext>();
        return services;
    }

    private static string GetRequiredConnectionString(this IServiceProvider provider)
    {
        var o = provider.GetRequiredService<IConfiguration>();
        var cs = o.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(cs))
        {
            throw new InvalidOperationException("Connection String cannot be null");
        }

        return cs;
    }
}