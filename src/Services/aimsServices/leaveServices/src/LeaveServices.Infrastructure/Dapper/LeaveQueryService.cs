using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LeaveServices.Infrastructure.Dapper;

/// <summary>
/// Lightweight Dapper query service for read-heavy, complex reporting queries.
/// </summary>
public sealed class LeaveQueryService
{
    private readonly string _connectionString;

    public LeaveQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("LeaveDb")
            ?? throw new InvalidOperationException("Connection string 'LeaveDb' is not configured.");
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<LeaveBalanceSummary>> GetLeaveBalanceSummaryAsync(long empSysId, int year)
    {
        const string sql = """
            SELECT
                lm.LEAVE_ID              AS LeaveId,
                lm.LEAVE_DESCRIPTION     AS LeaveDescription,
                ISNULL(lc.CREDIT_CREDITED,  0) AS Credited,
                ISNULL(lc.CREDIT_UTILIZED, 0) AS Utilized,
                ISNULL(lc.CREDIT_CLOSING,  0) AS Closing,
                ISNULL(lc.CREDIT_CREDITED - lc.CREDIT_UTILIZED, 0) AS Available
            FROM LEAVE_MASTER lm
            LEFT JOIN LEAVE_CREDIT lc
                ON lc.CREDIT_LEAVEID = lm.LEAVE_ID
               AND lc.CREDIT_EMPSYSID = @EmpSysId
               AND lc.CREDIT_YEAR     = @Year
            ORDER BY lm.LEAVE_ID
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync<LeaveBalanceSummary>(sql, new { EmpSysId = empSysId, Year = year });
    }

    public async Task<IEnumerable<LeaveApplicationSummary>> GetLeaveApplicationSummaryAsync(long empSysId)
    {
        const string sql = """
            SELECT
                ld.LEAVE_DETAILID    AS LeaveDetailId,
                lm.LEAVE_DESCRIPTION AS LeaveTypeName,
                ld.LEAVE_APPFROM     AS AppFrom,
                ld.LEAVE_APPTO       AS AppTo,
                ld.LEAVE_APPLIEDDAYS AS AppliedDays,
                ld.LEAVE_APPSTATUS   AS AppStatus,
                ld.LEAVE_REASON      AS Reason
            FROM LEAVE_DETAILS ld
            INNER JOIN LEAVE_MASTER lm ON lm.LEAVE_ID = ld.LEAVE_ID
            WHERE ld.LEAVE_EMPSYSID = @EmpSysId
            ORDER BY ld.LEAVE_ENTEREDON DESC
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync<LeaveApplicationSummary>(sql, new { EmpSysId = empSysId });
    }
}

public record LeaveBalanceSummary(
    long    LeaveId,
    string  LeaveDescription,
    decimal Credited,
    decimal Utilized,
    decimal Closing,
    decimal Available);

public record LeaveApplicationSummary(
    long     LeaveDetailId,
    string   LeaveTypeName,
    DateTime AppFrom,
    DateTime AppTo,
    decimal  AppliedDays,
    string   AppStatus,
    string?  Reason);
