using System.Dynamic;
using System.Text;
using Dapper;
using Domain.Aggregates.VendorManagement.Repositories.Dtos;
using Domain.Common.Extensions;
using Domain.Common.Types;
using Domain.Infrastructure;

namespace Persistence.Domain.VendorManagement.Cursors;

class VendorPagingDecorator : Paging<GetVendorPagingResult>
{
    private readonly GetVendorPagingDto _filters;
    public VendorPagingDecorator(
        GetVendorPagingDto filters,
        int totalCount,
        ISqlConnectionFactory connectionFactory)
        : base(totalCount, connectionFactory)
    {
        _filters = filters;
    }

    public static async Task<VendorPagingDecorator> Initialize(
        ISqlConnectionFactory connectionFactory,
        GetVendorPagingDto filter)
    {
        var count = await GetTotalCountAsync();
        return new(filter, count, connectionFactory);

        async Task<int> GetTotalCountAsync()
        {
            var queryBuilder = new StringBuilder("SELECT COUNT(VendorGuid) FROM Vendor ");
            BuildSearchFilterQuery(queryBuilder, filter, out var dao);

            using var conn = await connectionFactory.GetOpenConnection();

            var result = await conn.ExecuteScalarAsync<int>(queryBuilder.ToString(), (object)dao);

            await connectionFactory.CloseConnection(conn);
            return result;
        }
    }

    protected override async Task<IEnumerable<GetVendorPagingResult>> Fetch(int skip, int take)
    {
        const string query = "SELECT VendorGuid," +
                             "UserName AS Name," +
                             "ClientId," +
                             "ClientSecret," +
                             "VendorId," +
                             "Password," +
                             "Tenant FROM Vendor ";

        var queryBuilder = new StringBuilder(query);
        BuildSearchFilterQuery(queryBuilder, _filters, out var dao);

        if (take > 0)
        {
            queryBuilder.Append("LIMIT @pageSize ");
            dao.pageSize = take;
        }
        if (skip > 0)
        {
            queryBuilder.Append("OFFSET @offset");
            dao.offset = skip;
        }

        using var conn = await ConnectionFactory.GetOpenConnection();

        var result = await conn.QueryAsync<GetVendorPagingResult>(
            queryBuilder.ToString(),
            (object)dao);

        await ConnectionFactory.CloseConnection(conn);
        return result;
    }

    protected override ValueTask Dispose()
    {
        return ValueTask.CompletedTask;
    }

    private static void BuildSearchFilterQuery(
            StringBuilder filterQueryBuilder,
            GetVendorPagingDto filter,
            out dynamic parameters)
    {
        parameters = new ExpandoObject();
        var wheres = new List<string>();

        if (!filter.ClientSecret.IsEmpty())
        {
            wheres.Add("ClientSecret = @clientSecret");
            parameters.clientSecret = filter.ClientSecret;
        }
        if (!filter.ClientId.IsEmpty())
        {
            wheres.Add("ClientId = @clientId");
            parameters.clientId = filter.ClientId;
        }
        if (filter.VendorId > 0)
        {
            wheres.Add("VendorId = @vendorId");
            parameters.vendorId = filter.VendorId;
        }
        if (filter.Guid != Guid.Empty)
        {
            wheres.Add("Oid = @vendorGuid");
            parameters.vendorGuid = filter.Guid;
        }

        if (!wheres.Any()) return;

        filterQueryBuilder.Append(" WHERE ");
        for (int i = 0; i < wheres.Count; i++)
        {
            var w = wheres.ElementAt(i);
            filterQueryBuilder.Append(w);
            if (i < wheres.Count - 1)
            {
                filterQueryBuilder.Append(" AND ");
            }
        }
    }
}