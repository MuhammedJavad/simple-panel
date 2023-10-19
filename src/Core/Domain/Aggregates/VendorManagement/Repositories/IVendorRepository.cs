using Domain.Aggregates.VendorManagement.Repositories.Dtos;
using Domain.Common.Types;

namespace Domain.Aggregates.VendorManagement.Repositories;

public interface IVendorRepository
{
    Task<GetVendorKeysResult?> GetVendorKeys(Guid id);
    Task<GetVendorByIdResult?> GetVendorById(Guid id);
    Task<Paging<GetVendorPagingResult>> GetVendorPaging(GetVendorPagingDto filter);
}