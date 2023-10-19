using Domain.Aggregates.VendorManagement.ValueObjects;

namespace Domain.Aggregates.VendorManagement.Repositories.Dtos;

public record GetVendorByIdResult
{
    public Guid VendorGuid { get; init; } 
    public string UserName { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
    public long VendorId { get; init; }
    public string Password { get; init; } = default!;
    public VendorTenant Tenant { get; init; }
}