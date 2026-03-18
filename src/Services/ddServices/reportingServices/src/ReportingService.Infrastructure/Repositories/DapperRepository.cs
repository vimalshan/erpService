using Dapper;
using Microsoft.Data.SqlClient;

namespace ReportingService.Infrastructure.Repositories;

public class DapperRepository
{
    private readonly string _connectionString;

    public DapperRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryAsync<T>(sql, parameters);
        }
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}
