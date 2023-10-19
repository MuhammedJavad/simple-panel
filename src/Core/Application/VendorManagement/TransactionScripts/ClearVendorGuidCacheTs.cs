using Domain.Aggregates.VendorManagement.Repositories;
using Domain.Aggregates.VendorManagement.Repositories.Dtos;
using Domain.Common.Types;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.VendorManagement.TransactionScripts;

class ClearVendorGuidCacheTs
{
    private readonly IVendorRepository _repository;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CheckVendorGuidWithCoreTs> _logger;

    public ClearVendorGuidCacheTs(
        IVendorRepository repository,
        IDistributedCache distributedCache,
        ILogger<CheckVendorGuidWithCoreTs> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _repository = repository;
    }

    public async Task<R> Execute(Guid vendorGuid)
    {
        try
        {
            var result = await GetVendorKeys(vendorGuid);
            if (result == null) return VendorNotFound;
            await _distributedCache.RemoveAsync(result.VendorCode);
            await _distributedCache.RemoveAsync(result.VendorGuid.ToString());
            await _distributedCache.RemoveAsync($"{result.Tenant}_{result.VendorCode}");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while executing {nameof(ClearVendorGuidCacheTs)}");
            return CacheServerNotAvailable;
        }
    }

    private async Task<GetVendorKeysResult?> GetVendorKeys(Guid vendorGuid)
    {
        return await _repository.GetVendorKeys(vendorGuid);
    }
}