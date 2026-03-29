using Dapper;
using Microsoft.Data.SqlClient;

namespace HealthTransaction.Infrastructure.Dapper;

public class DapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(string connectionString) => _connectionString = connectionString;

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<T>(sql, parameters);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteAsync(sql, parameters);
    }
}
