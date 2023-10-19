using Domain.Aggregates.VendorManagement.Repositories.Dtos;
using Domain.ApplicationServices.VendorManagement.Dto;
using Domain.Common.Types;

namespace Domain.ApplicationServices.VendorManagement;

public interface IVendorService
{
    Task<R> InsertVendor(InsertVendorDto dto);
    Task<R> UpdateVendor(UpdateVendorDto dto);
    Task<R> ClearVendorGuidCache(Guid vendorGuid);
    Task<R<string>> CheckVendorGuidWithCore(Guid vendorGuid);
    Task<R<GetVendorByIdResult>> GetVendorById(Guid id);
    Task<R<Paging<GetVendorPagingResult>>> GetVendorPaging(GetVendorPagingDto pagingDto);
}