using Domain.Aggregates.VendorManagement.Entities;
using Domain.Aggregates.VendorManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Aggregates.VendorManagement;

public class VendorManagementAggregate
{
    private readonly Vendor _vendor;
    private VendorManagementAggregate(Vendor vendor)
    {
        _vendor = vendor;
    }

    public static async Task<VendorManagementAggregate?> LoadAsync(IVendorContext context, Guid id)
    {
        var vendor = await context.Vendor.FirstOrDefaultAsync(x => x.VendorGuid == id);
        return vendor == null ? default : new VendorManagementAggregate(vendor);
    }
    
    public static VendorManagementAggregate Create(
        Guid vendorGuid, 
        VendorTenant tenant,
        string username, 
        string clientId, 
        string clientSecret,
        string password,
        long? vendorId,
        long userId)
    {
        var vendor = Vendor.Create(vendorGuid, tenant, username, clientId, clientSecret, password, vendorId, userId);
        return new(vendor);
    }
    
    public void UpdateVendor(
        Guid id, 
        VendorTenant tenant,
        string username, 
        string clientId, 
        string clientSecret,
        string password,
        long? vendorId,
        long userId)
    {
        _vendor.Update(id, tenant, username, clientId, clientSecret, password, vendorId, userId);
    }

    public async Task AddAsync(IVendorContext context)
    {
        await context.Vendor.AddAsync(_vendor);
    }
    
    public void UpdateAsync(IVendorContext context)
    {
        context.Vendor.Update(_vendor);
    }
}