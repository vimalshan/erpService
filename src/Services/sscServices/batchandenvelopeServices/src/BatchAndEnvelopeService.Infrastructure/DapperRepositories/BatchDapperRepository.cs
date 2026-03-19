using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Infrastructure.DapperRepositories;

public class BatchDapperRepository
{
    private readonly string _connectionString;

    public BatchDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<BatchDto>> GetBatchSummaryAsync(int page = 1, int pageSize = 20)
    {
        using var conn = CreateConnection();
        const string sql = @"
            SELECT b.BATCH_ID as BatchId,
                   b.BATCH_CREATEDBY as CreatedBy,
                   b.BATCH_CREATEDON as CreatedOn,
                   b.BATCH_LOCATIONID as LocationId,
                   b.BATCH_RECEIVEDBY as ReceivedBy,
                   b.BATCH_RECEIVEDON as ReceivedOn,
                   b.BATCH_PODNO as PodNo,
                   b.BATCH_SUMMARYFLAG as SummaryFlag,
                   b.BATCH_CANCELBY as CancelBy,
                   b.BATCH_CANCELDATE as CancelDate,
                   b.BATCH_CONFIRMEDBY as ConfirmedBy,
                   b.BATCH_CONFIRMEDON as ConfirmedOn,
                   b.BATCH_COURIERNAME as CourierName,
                   b.BATCH_SCANFLAG as ScanFlag
            FROM BATCH_MAIN b
            ORDER BY b.BATCH_CREATEDON DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var rows = await conn.QueryAsync<dynamic>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
        return rows.Select(r => new BatchDto(
            r.BatchId, r.CreatedBy, r.CreatedOn, r.LocationId, r.ReceivedBy, r.ReceivedOn,
            r.PodNo, r.SummaryFlag, (long?)r.CancelBy, (DateTime?)r.CancelDate,
            (long?)r.ConfirmedBy, (DateTime?)r.ConfirmedOn, (string?)r.CourierName, r.ScanFlag,
            Enumerable.Empty<BatchDetailDto>()));
    }

    public async Task<int> GetBatchCountByLocationAsync(long locationId)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM BATCH_MAIN WHERE BATCH_LOCATIONID = @LocationId",
            new { LocationId = locationId });
    }
}
