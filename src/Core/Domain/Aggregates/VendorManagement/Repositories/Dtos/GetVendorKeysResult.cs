using Domain.Aggregates.VendorManagement.ValueObjects;

namespace Domain.Aggregates.VendorManagement.Repositories.Dtos;

public record GetVendorKeysResult
{
    public string VendorCode { get; set; } = default!;
    public Guid VendorGuid { get; set; }
    public VendorTenant Tenant { get; set; }
}