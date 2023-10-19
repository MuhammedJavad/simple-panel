using Domain.Aggregates.UserManagement.Entities;
using Domain.Common.Types;
using Microsoft.EntityFrameworkCore;

namespace Domain.Aggregates.UserManagement;

public interface IUserContext : IDbContextBase
{
    public DbSet<User> Users { get; set; }
}