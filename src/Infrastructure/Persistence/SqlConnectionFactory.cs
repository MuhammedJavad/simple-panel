
using System.Data;
using System.Data.Common;
using Domain.Infrastructure;
using MySql.Data.MySqlClient;

namespace Persistence;

class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<DbConnection> GetOpenConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task CloseConnection(DbConnection connection)
    {
        if (connection.State == ConnectionState.Closed) return;
        await connection.CloseAsync();
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }
}