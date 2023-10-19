using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Aggregates.VendorManagement.ValueObjects;
using Domain.Common.Validators;

namespace Domain.ApplicationServices.VendorManagement.Dto;

public record InsertVendorDto
{
    public static readonly InsertVendorDto Empty = new()
    {
        UserName = string.Empty,
        ClientId = string.Empty,
        ClientSecret = string.Empty,
        Password = string.Empty,
    };

    [Required]
    [NotEmpty]
    [DisplayName("Vendor Guid")]
    public Guid VendorGuid { get; set; }
    
    [Required]
    [DisplayName("Username")]
    public string UserName { get; set; } = default!;
    
    [Required]
    [DisplayName("Client Id")]
    public string ClientId { get; set; } = default!;
    
    [Required]
    [DisplayName("Client Secret")]
    public string ClientSecret { get; set; } = default!;
    
    [Required]
    [DisplayName("Password")]
    public string Password { get; set; } = default!;
    
    [RequiredIf(
        "Tenant", 
        VendorTenant.SnappExpress, 
        ErrorMessage = "Vendor Id Cannot be 0 or empty")]
    public long? VendorId { get; set; }
    
    [Required]
    [DisplayName("Gateway")]
    public VendorTenant Tenant { get; set; }

    public long UserId { get; set; }
}

public record UpdateVendorDto : InsertVendorDto
{
    public UpdateVendorDto(Guid currentOid, InsertVendorDto dto)
    {
        CurrentOid = currentOid;
        Password = dto.Password;
        VendorId = dto.VendorId;
        Tenant = dto.Tenant;
        ClientSecret = dto.ClientSecret;
        ClientId = dto.ClientId;
        UserName = dto.UserName;
        VendorGuid = dto.VendorGuid;
        UserId = dto.UserId;
    }

    public Guid CurrentOid { get; }
}