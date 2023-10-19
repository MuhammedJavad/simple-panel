using Microsoft.EntityFrameworkCore;

namespace Domain.Common.Types;

public interface IDbContextBase
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    bool IsUniqueConstraintException(DbUpdateException ex, out string columnName);
}