using Domain.Aggregates.VendorManagement.Entities;
using Domain.Common.Types;
using Microsoft.EntityFrameworkCore;

namespace Domain.Aggregates.VendorManagement;

public interface IVendorContext : IDbContextBase
{
    public DbSet<Vendor> Vendor { get; set; }
}