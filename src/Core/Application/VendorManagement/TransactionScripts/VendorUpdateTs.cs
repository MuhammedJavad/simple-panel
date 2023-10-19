using Domain.Aggregates.VendorManagement;
using Domain.ApplicationServices.VendorManagement.Dto;
using Domain.Common.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.VendorManagement.TransactionScripts;

class VendorUpdateTs
{
    private readonly IVendorContext _context;
    private readonly ILogger<VendorUpdateTs> _logger;

    public VendorUpdateTs(IVendorContext context, ILogger<VendorUpdateTs> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<R> Execute(UpdateVendorDto dto)
    {
        try
        {
            var aggregate = await VendorManagementAggregate.LoadAsync(_context, dto.CurrentOid);
            if (aggregate == null) return VendorNotFound;
            
            aggregate.UpdateVendor(dto.VendorGuid, 
                dto.Tenant,
                dto.UserName, 
                dto.ClientId, 
                dto.ClientSecret, 
                dto.Password, 
                dto.VendorId,
                dto.UserId);
            aggregate.UpdateAsync(_context);
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DomainException e)
        {
            return e.Code;
        }
        catch (DbUpdateException ex)
        {
            return _context.IsUniqueConstraintException(ex, out var columnName) 
                ? new R(false, $"{columnName} already exists") 
                : false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while executing {nameof(VendorUpdateTs)}");
            return false;
        }
    }
}