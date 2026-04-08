using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SciTransactional.Application.Interfaces;

namespace SciTransactional.Infrastructure.Dapper;

public sealed class DapperContext(IConfiguration configuration) : IDapperContext
{
    private readonly string _connectionString =
        configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection string is not configured.");

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IReadOnlyList<T>> QueryStoredProcAsync<T>(
        string storedProcedure, object? parameters = null)
    {
        using var connection = CreateConnection();
        var result = await connection.QueryAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        return result.ToList().AsReadOnly();
    }

    public async Task<T?> QuerySingleStoredProcAsync<T>(
        string storedProcedure, object? parameters = null)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
