using Application;
using Blazored.Toast;
using Infrastructure;
using Blazor.Extensions;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder
    .UseOpenTelemetry()
    .AddAuth();

builder.Services
    .AddBlazoredToast()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPersistence()
    .AddHealthChecks();

builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapFallbackToPage("/_Host");
app.MapHealthChecks("/hc");
await app.RunAsync();