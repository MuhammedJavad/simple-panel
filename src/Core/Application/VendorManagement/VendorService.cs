using Domain.Aggregates.VendorManagement.Repositories.Dtos;
using Application.VendorManagement.TransactionScripts;
using Domain.Aggregates.VendorManagement.Repositories;
using Domain.ApplicationServices.VendorManagement.Dto;
using Domain.ApplicationServices.VendorManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Domain.Common.Types;

namespace Application.VendorManagement;

class VendorService : IVendorService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<VendorService> _logger;

    public VendorService(IServiceProvider provider, ILogger<VendorService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public Task<R> InsertVendor(InsertVendorDto dto)
    {
        return _provider.GetRequiredService<VendorInsertTs>().Execute(dto);
    }

    public Task<R> UpdateVendor(UpdateVendorDto dto)
    {
        return _provider.GetRequiredService<VendorUpdateTs>().Execute(dto);
    }

    public Task<R> ClearVendorGuidCache(Guid vendorGuid)
    {
        return _provider.GetRequiredService<ClearVendorGuidCacheTs>().Execute(vendorGuid);
    }

    public Task<R<string>> CheckVendorGuidWithCore(Guid vendorGuid)
    {
        return _provider.GetRequiredService<CheckVendorGuidWithCoreTs>().Execute(vendorGuid);
    }

    public async Task<R<GetVendorByIdResult>> GetVendorById(Guid id)
    {
        // todo; think about this try/catch, deduplicate code
        try
        {
            var service = _provider.GetRequiredService<IVendorRepository>();
            var vendor = await service.GetVendorById(id);
            if (vendor == null) return VendorNotFound;
            return vendor;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while trying to get {nameof(Paging<GetVendorPagingResult>)}");
            return Failed;
        }
    }

    public async Task<R<Paging<GetVendorPagingResult>>> GetVendorPaging(GetVendorPagingDto pagingDto)
    {
        try
        {
            var service = _provider.GetRequiredService<IVendorRepository>();
            return await service.GetVendorPaging(pagingDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while trying to get {nameof(Paging<GetVendorPagingResult>)}");
            return Failed;
        }
    }
}