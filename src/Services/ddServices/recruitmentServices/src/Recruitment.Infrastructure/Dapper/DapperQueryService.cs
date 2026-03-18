using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Recruitment.Infrastructure.Dapper;

/// <summary>
/// Dapper-based queries for complex/optimized reads
/// </summary>
public class DapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, parameters);
        }
    }

    public async Task <T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
    }

    public async Task<int> ExecuteAsync(string sql, object parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}
