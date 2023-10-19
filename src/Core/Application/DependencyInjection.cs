using Application.UserManagement;
using Application.UserManagement.TransactionScripts.Login;
using Application.VendorManagement;
using Application.VendorManagement.TransactionScripts;
using Domain.ApplicationServices.UserManagement;
using Domain.ApplicationServices.VendorManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<AuthenticateBaseTs, NormalAuthenticateTs>();
        service.AddScoped<IAccountService, AccountService>();
        service.AddScoped<IVendorService, VendorService>();
        service.AddScoped<VendorInsertTs>();
        service.AddScoped<VendorUpdateTs>();
        service.AddScoped<ClearVendorGuidCacheTs>();
        service.AddScoped<CheckVendorGuidWithCoreTs>();
        service.AddHttpClient<CheckVendorGuidWithCoreTs>()
            .ConfigureHttpClient((p, client) =>
            {
                var configuration = p.GetRequiredService<IConfiguration>();
                var url = configuration.GetConnectionString("CoreBaseUrl");
                if (string.IsNullOrWhiteSpace(url)) throw new InvalidOperationException("Please add core url");
                client.BaseAddress = new Uri(url);
            });
        return service;
    }
}