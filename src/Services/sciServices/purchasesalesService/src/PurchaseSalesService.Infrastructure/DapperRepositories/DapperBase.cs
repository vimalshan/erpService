using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PurchaseSalesService.Infrastructure.DapperRepositories;

public abstract class DapperBase
{
    private readonly string _connectionString;

    protected DapperBase(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<T>(sql, param);
    }

    protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    protected async Task<int> ExecuteAsync(string sql, object? param = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteAsync(sql, param);
    }
}
