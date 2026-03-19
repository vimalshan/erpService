using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Infrastructure.DapperRepositories;

public class EnvelopeDapperRepository
{
    private readonly string _connectionString;

    public EnvelopeDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<EnvelopeDto>> GetEnvelopeSummaryAsync(int page = 1, int pageSize = 20)
    {
        using var conn = CreateConnection();
        const string sql = @"
            SELECT e.ENV_ID as EnvelopeId,
                   e.ENV_TYPE as EnvelopeType,
                   e.ENV_CREATEDBY as CreatedBy,
                   e.ENV_CREATEDON as CreatedOn,
                   e.ENV_RECEIVEDBY as ReceivedBy,
                   e.ENV_RECEIVEDON as ReceivedOn,
                   e.ENV_SUMMARYFLAG as SummaryFlag,
                   e.ENV_CANCELLEDBY as CancelledBy,
                   e.ENV_CANCELLEDON as CancelledOn,
                   e.ENV_CONFIRMEDBY as ConfirmedBy,
                   e.ENV_CONFIRMEDON as ConfirmedOn,
                   e.ENV_SCANLOTNO as ScanLotNo,
                   e.ENV_LOCID as LocationId
            FROM ENV_MAIN e
            ORDER BY e.ENV_CREATEDON DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var rows = await conn.QueryAsync<dynamic>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
        return rows.Select(r => new EnvelopeDto(
            r.EnvelopeId, r.EnvelopeType, r.CreatedBy, r.CreatedOn, (long?)r.ReceivedBy,
            (DateTime?)r.ReceivedOn, r.SummaryFlag, (long?)r.CancelledBy, (DateTime?)r.CancelledOn,
            (long?)r.ConfirmedBy, (DateTime?)r.ConfirmedOn, (long?)r.ScanLotNo, r.LocationId,
            Enumerable.Empty<EnvelopeDetailDto>()));
    }
}
