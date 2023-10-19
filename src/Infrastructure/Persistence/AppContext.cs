using System.Reflection;
using Domain.Aggregates.UserManagement;
using Domain.Aggregates.UserManagement.Entities;
using Domain.Aggregates.VendorManagement;
using Domain.Aggregates.VendorManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence;

class AppContext : DbContext, IUserContext, IVendorContext
{
    public AppContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Vendor> Vendor { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public bool IsUniqueConstraintException(DbUpdateException ex, out string columnName)
    {
        return ex.IsUniqueConstraintException(out columnName);
    }
}