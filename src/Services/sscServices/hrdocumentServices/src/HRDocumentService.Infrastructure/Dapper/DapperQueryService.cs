using System.Data;
using Dapper;
using HRDocumentService.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRDocumentService.Infrastructure.Dapper;

public sealed class DapperQueryService(IConfiguration configuration) : IDapperQueryService
{
    private IDbConnection CreateConnection()
    {
        return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? parameters = null, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        var result = await connection.QueryAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: ct));
        return result.ToList().AsReadOnly();
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: ct));
    }
}
