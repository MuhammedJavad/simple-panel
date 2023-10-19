using System.Data.Common;

namespace Domain.Infrastructure;

public interface ISqlConnectionFactory
{
    Task<DbConnection> GetOpenConnection();
    Task CloseConnection(DbConnection connection);
    string GetConnectionString();
}