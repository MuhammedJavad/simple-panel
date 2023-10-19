using Domain.Aggregates.VendorManagement.ValueObjects;

namespace Domain.Aggregates.VendorManagement.Repositories.Dtos;

public record GetVendorPagingDto
{
    public Guid Guid { get; set; }
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public long VendorId { get; set; }
}

public record GetVendorPagingResult
{
    public Guid VendorGuid { get; set; }
    public string Name { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public long VendorId { get; set; }
    public string Password { get; set; } = default!;
    public VendorTenant Tenant { get; set; }
}