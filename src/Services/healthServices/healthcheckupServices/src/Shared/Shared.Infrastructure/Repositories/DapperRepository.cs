using Dapper;
using System.Data;
using Shared.Core.Repositories;

namespace Shared.Infrastructure.Repositories;

/// <summary>
/// Dapper-based repository for high-performance data access
/// Useful for complex queries and read-heavy scenarios
/// </summary>
public abstract class DapperRepository<iT> where iT : class
{
    protected readonly IDbConnection _dbConnection;

    protected DapperRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        return await _dbConnection.QueryAsync<T>(sql, parameters);
    }

    protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    protected async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        return await _dbConnection.ExecuteAsync(sql, parameters);
    }

    protected async Task<IEnumerable<dynamic>> QueryDynamicAsync(string sql, object? parameters = null)
    {
        return await _dbConnection.QueryAsync(sql, parameters);
    }
}
