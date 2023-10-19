using Domain.Aggregates.VendorManagement.ValueObjects;
using Domain.Common.Types;

namespace Domain.Aggregates.VendorManagement.Entities;

public class Vendor
{
    public long Id { get; set; }
    public Guid VendorGuid { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string VendorCode { get; set; } = default!;
    public VendorTenant Tenant { get; set; }
    public long? VendorId { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public long UpdatedByUserId { get; set; }

    internal void Update(
        Guid vendorGuid, 
        VendorTenant tenant,
        string username, 
        string clientId, 
        string clientSecret,
        string password,
        long? vendorId,
        long userId)
    {
        Check.NotEmpty(clientId, ClientIdCannotBeNull);
        Check.NotEmpty(password, PasswordCannotBeNull);
        Check.NotEmpty(clientSecret, ClientSecretCannotBeNull);
        Check.If(tenant == VendorTenant.SnappExpress && !vendorId.HasValue, VendorIdCannotBeNull);
        
        VendorGuid = vendorGuid;
        UserName = username;
        Password = password;
        ClientId = clientId;
        VendorCode = clientId;
        ClientSecret = clientSecret;
        VendorId = vendorId;
        Tenant = tenant;
        UpdatedByUserId = userId;
        LastModifiedDate = DateTime.Now;
    }
    
    internal static Vendor Create(
        Guid vendorGuid,
        VendorTenant tenant,
        string username,
        string clientId,
        string clientSecret,
        string password,
        long? vendorId,
        long userId)
    {
        Check.NotEmpty(clientId, ClientIdCannotBeNull);
        Check.NotEmpty(password, PasswordCannotBeNull);
        Check.NotEmpty(clientSecret, ClientSecretCannotBeNull);
        Check.If(tenant == VendorTenant.SnappExpress && !vendorId.HasValue, VendorIdCannotBeNull);
        
        return new()
        {
            Tenant = tenant,
            VendorGuid = vendorGuid,
            Password = password,
            ClientId = clientId,
            VendorCode = clientId,
            ClientSecret = clientSecret,
            UserName = username,
            VendorId = vendorId,
            UpdatedByUserId = userId,
            LastModifiedDate = DateTime.Now
        };
    }
}