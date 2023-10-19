using Dapper;
using Domain.Common.Types;
using Domain.Infrastructure;
using Persistence.Domain.VendorManagement.Cursors;
using Domain.Aggregates.VendorManagement.Repositories;
using Domain.Aggregates.VendorManagement.Repositories.Dtos;

namespace Persistence.Domain.VendorManagement.Repositories;

class VendorRepository : IVendorRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public VendorRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<GetVendorKeysResult?> GetVendorKeys(Guid id)
    {
        const string query = "SELECT VendorGuid," +
                             "VendorCode," +
                             "Tenant FROM Vendor " +
                             "WHERE VendorGuid = @oid";
        using var conn = await _connectionFactory.GetOpenConnection();
        
        var result = await conn.QueryFirstOrDefaultAsync<GetVendorKeysResult>(query, new { oid = id });
        
        await _connectionFactory.CloseConnection(conn);

        return result;
    }

    public async Task<GetVendorByIdResult?> GetVendorById(Guid id)
    {
        // Todo; ArgumentException.ThrowIfNullOrEmpty(id);
        
        const string query = "SELECT VendorGuid," +
                             "UserName," +
                             "ClientId," +
                             "ClientSecret," +
                             "VendorId," +
                             "Password," +
                             "Tenant FROM Vendor " +
                             "WHERE VendorGuid = @oid";
        using var conn = await _connectionFactory.GetOpenConnection();
        
        var result = await conn.QueryFirstOrDefaultAsync<GetVendorByIdResult>(query, new { oid = id });
        
        await _connectionFactory.CloseConnection(conn);
        return result;
    }

    public async Task<Paging<GetVendorPagingResult>> GetVendorPaging(GetVendorPagingDto filter)
    {
        return await VendorPagingDecorator.Initialize(_connectionFactory, filter);
    }
}