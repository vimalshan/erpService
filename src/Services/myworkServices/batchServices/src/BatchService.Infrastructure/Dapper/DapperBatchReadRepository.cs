using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BatchService.Domain.Entities;

namespace BatchService.Infrastructure.Dapper;

/// <summary>
/// Lightweight read-only Dapper repository for high-performance queries.
/// Bypasses EF Core change tracking for reporting use-cases.
/// </summary>
public sealed class DapperBatchReadRepository
{
    private readonly string _connectionString;

    public DapperBatchReadRepository(string connectionString)
        => _connectionString = connectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<BatchSummary>> GetSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT BATCH_ID           AS BatchId,
                   BATCH_MONTHNO      AS BatchMonthNo,
                   BATCH_STATUS       AS BatchStatus,
                   BATCH_LASTMODIFIEDBY AS BatchLastModifiedBy,
                   BATCH_LASTMODIFIEDON AS BatchLastModifiedOn
            FROM   BATCH_MASTER
            ORDER  BY BATCH_ID;
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync<BatchSummary>(new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<BatchSummary?> GetByIdAsync(long batchId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT BATCH_ID           AS BatchId,
                   BATCH_MONTHNO      AS BatchMonthNo,
                   BATCH_STATUS       AS BatchStatus,
                   BATCH_LASTMODIFIEDBY AS BatchLastModifiedBy,
                   BATCH_LASTMODIFIEDON AS BatchLastModifiedOn
            FROM   BATCH_MASTER
            WHERE  BATCH_ID = @BatchId;
            """;

        using var conn = CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<BatchSummary>(
            new CommandDefinition(sql, new { BatchId = batchId }, cancellationToken: ct));
    }
}

/// <summary>Flat record used by Dapper projections.</summary>
public sealed record BatchSummary(
    long     BatchId,
    int      BatchMonthNo,
    char     BatchStatus,
    long     BatchLastModifiedBy,
    DateTime BatchLastModifiedOn);
