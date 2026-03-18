using Dapper;
using LoanService.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace LoanService.Infrastructure.Persistence.Dapper;

public class LoanDapperRepository : ILoanDapperRepository
{
    private readonly string _connectionString;

    public LoanDapperRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: ct));
    }
}
