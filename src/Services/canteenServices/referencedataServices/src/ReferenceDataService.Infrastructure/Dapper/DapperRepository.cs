using Dapper;
using System.Data;

namespace ReferenceDataService.Infrastructure.Dapper;

public class DapperRepository
{
    private readonly DapperContext _context;

    public DapperRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }
}
