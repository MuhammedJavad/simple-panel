using Domain.Aggregates.VendorManagement;
using Domain.ApplicationServices.VendorManagement.Dto;
using Domain.Common.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.VendorManagement.TransactionScripts;

class VendorInsertTs
{
    private readonly IVendorContext _vendorContext;
    private readonly ILogger<VendorInsertTs> _logger;
    
    public VendorInsertTs(ILogger<VendorInsertTs> logger, IVendorContext vendorContext)
    {
        _logger = logger;
        _vendorContext = vendorContext;
    }

    public async Task<R> Execute(InsertVendorDto dto)
    {
        try
        {
            var exists = await _vendorContext.Vendor.AnyAsync(x => x.VendorGuid == dto.VendorGuid);
            if (exists) return VendorAlreadyExists;

            var aggregate = VendorManagementAggregate.Create(
                dto.VendorGuid,
                dto.Tenant,
                dto.UserName,
                dto.ClientId,
                dto.ClientSecret,
                dto.Password,
                dto.VendorId,
                dto.UserId);

            await aggregate.AddAsync(_vendorContext);
            await _vendorContext.SaveChangesAsync();
            return true;
        }
        catch (DomainException e)
        {
            return e.Code;
        }
        catch (DbUpdateException ex)
        {
            return _vendorContext.IsUniqueConstraintException(ex, out var columnName) 
                ? new R(false, $"{columnName} already exists") 
                : false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while executing VendorInsertion ts");           
            return false;
        }
    }
}