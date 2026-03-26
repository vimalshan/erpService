using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Infrastructure.Services;

public class AimsReadService(IConfiguration configuration)
{
    private SqlConnection CreateConnection() =>
        new(configuration.GetConnectionString("AimsTransactionDb"));

    public async Task<IEnumerable<SwipeDto>> GetSwipesByDateRangeAsync(
        long employeeSysId, DateTime from, DateTime to)
    {
        await using var conn = CreateConnection();
        const string sql = """
            SELECT
                SRP_SYSID       AS SwipeId,
                SRP_EMPSYSID    AS EmployeeSysId,
                SRP_PUNCHTIME   AS PunchTime,
                SRP_INOUTSTATUS AS PunchStatus,
                SRP_GATENO      AS GateNo,
                SRP_MACHINENO   AS MachineNo,
                SRP_REFERENCENO AS ReferenceNo,
                SRP_PULLSTATUS  AS PullStatus,
                SRP_UPDATEDON   AS EnteredOn
            FROM SWIPE_RAWPUNCH
            WHERE SRP_EMPSYSID = @EmployeeSysId
              AND SRP_PUNCHTIME BETWEEN @From AND @To
            ORDER BY SRP_PUNCHTIME DESC
        """;

        var rows = await conn.QueryAsync<SwipeDto>(
            sql,
            new { EmployeeSysId = employeeSysId, From = from, To = to },
            commandTimeout: 30);

        return rows;
    }

    public async Task<AttendanceSummaryDto?> GetAttendanceSummaryAsync(
        long employeeSysId, DateTime monthStart, DateTime monthEnd)
    {
        await using var conn = CreateConnection();
        const string sql = """
            SELECT
                ATS_SYSID       AS SummaryId,
                ATS_EMPSYSID    AS EmployeeSysId,
                ATS_MONTHSTART  AS MonthStart,
                ATS_MONTHEND    AS MonthEnd,
                ATS_WORKINGDAYS AS WorkingDays,
                ATS_PRESENTDAYS AS PresentDays,
                ATS_ABSENTDAYS  AS AbsentDays,
                ATS_OTHOURS     AS OvertimeHours,
                ATS_LOPDAYS     AS LopDays
            FROM ATTENDANCE_SUMMARY
            WHERE ATS_EMPSYSID = @EmployeeSysId
              AND ATS_MONTHSTART = @MonthStart
              AND ATS_MONTHEND = @MonthEnd
        """;

        return await conn.QueryFirstOrDefaultAsync<AttendanceSummaryDto>(
            sql,
            new { EmployeeSysId = employeeSysId, MonthStart = monthStart, MonthEnd = monthEnd },
            commandTimeout: 30);
    }
}
