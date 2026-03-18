using Dapper;
using LeaveServices.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LeaveServices.Infrastructure.Dapper;

/// <summary>Read-only Dapper queries for high-performance reporting scenarios.</summary>
public interface ILeaveReadRepository
{
    Task<IEnumerable<LeaveEncashmentDto>> GetEncashmentsByStatusAsync(char? status, CancellationToken ct = default);
    Task<IEnumerable<LossOfPayDto>> GetLopSummaryByMonthAsync(DateOnly month, CancellationToken ct = default);
    Task<IEnumerable<LeaveRequestDto>> GetAllLeaveRequestsPagedAsync(int page, int pageSize, CancellationToken ct = default);
}

public sealed class LeaveReadRepository : ILeaveReadRepository
{
    private readonly string _connectionString;

    public LeaveReadRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("LeaveDb")
            ?? throw new InvalidOperationException("Connection string 'LeaveDb' not found.");
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<LeaveEncashmentDto>> GetEncashmentsByStatusAsync(char? status, CancellationToken ct)
    {
        const string sql = """
            SELECT ENCASHMENT_ID        AS EncashmentId,
                   EMP_SYS_ID           AS EmpSysId,
                   LEAVE_TYPE           AS LeaveType,
                   ENCASHMENT_DAYS      AS EncashmentDays,
                   ENCASHMENT_AMOUNT    AS EncashmentAmount,
                   REQUEST_DATE         AS RequestDate,
                   ENCASHMENT_STATUS    AS EncashmentStatus,
                   CREATED_ON           AS CreatedOn
            FROM   LEAVE_ENCASHMENT
            WHERE  (@Status IS NULL OR ENCASHMENT_STATUS = @Status)
            ORDER  BY REQUEST_DATE DESC
            """;

        await using var conn = CreateConnection();
        var rows = await conn.QueryAsync<dynamic>(new CommandDefinition(sql, new { Status = status }, cancellationToken: ct));
        return rows.Select(r => new LeaveEncashmentDto(
            (long)r.EncashmentId, (long)r.EmpSysId, (string)r.LeaveType,
            (int)r.EncashmentDays, (decimal)r.EncashmentAmount,
            DateOnly.FromDateTime((DateTime)r.RequestDate),
            Convert.ToChar(r.EncashmentStatus),
            GetStatusDescription(Convert.ToChar(r.EncashmentStatus)),
            (DateTime)r.CreatedOn));
    }

    public async Task<IEnumerable<LossOfPayDto>> GetLopSummaryByMonthAsync(DateOnly month, CancellationToken ct)
    {
        const string sql = """
            SELECT LOP_ID       AS LopId,
                   EMP_SYS_ID   AS EmpSysId,
                   LOP_DAYS     AS LopDays,
                   LOP_MONTH    AS LopMonth,
                   LOP_REMARKS  AS LopRemarks,
                   CREATED_ON   AS CreatedOn
            FROM   LOSS_OF_PAY
            WHERE  YEAR(LOP_MONTH) = @Year AND MONTH(LOP_MONTH) = @Month
            ORDER  BY EMP_SYS_ID
            """;

        await using var conn = CreateConnection();
        var rows = await conn.QueryAsync<dynamic>(new CommandDefinition(sql,
            new { Year = month.Year, Month = month.Month }, cancellationToken: ct));
        return rows.Select(r => new LossOfPayDto(
            (long)r.LopId, (long)r.EmpSysId, (int)r.LopDays,
            DateOnly.FromDateTime((DateTime)r.LopMonth),
            (string?)r.LopRemarks, (DateTime)r.CreatedOn));
    }

    public async Task<IEnumerable<LeaveRequestDto>> GetAllLeaveRequestsPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        const string sql = """
            SELECT REQ_NUM        AS ReqNum,
                   FINYEAR_SRLNO AS FinyearSrlno,
                   EMP_USERID    AS EmpUserId,
                   SUP_USERID    AS SupUserId,
                   REQ_DATE      AS ReqDate
            FROM   LET_MAIN
            ORDER  BY REQ_DATE DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var conn = CreateConnection();
        var rows = await conn.QueryAsync<dynamic>(new CommandDefinition(sql,
            new { Offset = (page - 1) * pageSize, PageSize = pageSize }, cancellationToken: ct));
        return rows.Select(r => new LeaveRequestDto(
            (long)r.ReqNum, (int)r.FinyearSrlno, (string)r.EmpUserId,
            (string?)r.SupUserId, (DateTime?)r.ReqDate, []));
    }

    private static string GetStatusDescription(char code) => code switch
    {
        'P' => "Pending",
        'A' => "Approved",
        'R' => "Rejected",
        'D' => "Processed",
        _ => "Unknown"
    };
}
