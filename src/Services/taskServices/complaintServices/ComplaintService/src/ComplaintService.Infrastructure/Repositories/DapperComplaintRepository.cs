using ComplaintService.Application.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ComplaintService.Infrastructure.Repositories;

/// <summary>Read-side Dapper queries for reports and complex joins.</summary>
public class DapperComplaintRepository(IConfiguration configuration)
{
    private SqlConnection CreateConnection() =>
        new(configuration.GetConnectionString("TaskDb"));

    public async Task<IEnumerable<ComplaintTicketDto>> GetComplaintSummaryAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT
                CD_TICKET_NUM   AS TicketNum,
                CD_GROUPID      AS GroupId,
                CD_TYPE         AS Type,
                CD_LOCATION     AS Location,
                CD_DEPARTMENT   AS Department,
                CD_PROCESS      AS Process,
                CD_SUBJECT      AS Subject,
                CD_DESCRIPTION  AS Description,
                CD_NCR          AS IsNCR,
                CD_TARGET_DATE  AS TargetDate,
                CD_CLOSURE_DATE AS ClosureDate,
                CASE WHEN CD_CLOSURE_DATE IS NOT NULL THEN 1 ELSE 0 END AS IsClosed
            FROM COMPL_DET
            ORDER BY CD_TICKET_NUM DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        await using var conn = CreateConnection();
        return await conn.QueryAsync<ComplaintTicketDto>(
            new CommandDefinition(sql,
                new { Offset = (page - 1) * pageSize, PageSize = pageSize },
                cancellationToken: ct));
    }

    public async Task<string> GetComplaintStatusAsync(decimal ticketNum, CancellationToken ct = default)
    {
        const string sql = "SELECT dbo.fn_GetComplaintStatus(@TicketNum)";
        await using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<string>(
            new CommandDefinition(sql, new { TicketNum = ticketNum }, cancellationToken: ct))
            ?? "Unknown";
    }
}
