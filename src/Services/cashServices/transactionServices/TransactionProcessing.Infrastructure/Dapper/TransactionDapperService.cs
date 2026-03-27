using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TransactionProcessing.Infrastructure.Dapper;

public sealed class TransactionDapperService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<T> ExecuteStoredProcAsync<T>(string procName, object? param = null, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstAsync<T>(
            new CommandDefinition(procName, param, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }
}
