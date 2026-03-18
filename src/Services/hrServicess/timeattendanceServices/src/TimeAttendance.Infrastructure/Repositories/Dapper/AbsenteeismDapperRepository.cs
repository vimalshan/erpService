using Dapper;
using Microsoft.Data.SqlClient;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.Infrastructure.Repositories.Dapper;

/// <summary>
/// Read-only Dapper queries for complex / performance-sensitive read scenarios.
/// </summary>
public class AbsenteeismDapperRepository(string connectionString)
{
    public async Task<IEnumerable<AbsenteeismSummaryDto>> GetAbsenteeismSummaryByUnitAsync(
        long unitId, int year, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                ABS_UNITID   AS UnitId,
                ABS_YEAR     AS Year,
                ABS_MONTH    AS Month,
                SUM(ABS_TOTMANDAYS)  AS TotalManDays,
                SUM(ABS_ABSMANDAYS)  AS TotalAbsentDays,
                CASE WHEN SUM(ABS_TOTMANDAYS) = 0 THEN 0
                     ELSE ROUND(CAST(SUM(ABS_ABSMANDAYS) AS DECIMAL(18,2))
                          / SUM(ABS_TOTMANDAYS) * 100, 2) END AS OverallAbsenteeismRate
            FROM ABSENTEEISM_DET
            WHERE ABS_UNITID = @UnitId AND ABS_YEAR = @Year
            GROUP BY ABS_UNITID, ABS_YEAR, ABS_MONTH
            ORDER BY ABS_MONTH
            """;

        await using var connection = new SqlConnection(connectionString);
        var command = new CommandDefinition(sql,
            new { UnitId = unitId, Year = year },
            cancellationToken: cancellationToken);

        return await connection.QueryAsync<AbsenteeismSummaryDto>(command);
    }

    public async Task<IEnumerable<AbsenteeismMisDto>> GetMisReportByUnitAndMonthAsync(
        int unitId, string month, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                ABSID AS Id, UNTID AS UnitId, CID AS CompanyId, DID AS DepartmentId,
                SYSID AS SystemId, GRD AS Grade, PLD AS PlannedLeave, PDS AS PaidDays,
                WOFF AS WeeklyOff, LWOP AS LeaveWithoutPay, NPH AS NumberOfPresentHours,
                COF AS CompensatoryOff, BKL AS BankLeave, APL AS AnnualPaidLeave,
                PNL AS PenaltyLeave, SWP AS ShiftSwap, OND AS OnDuty,
                MNTH AS Month, LOGSYSID AS LogSystemId, LWOPP AS LeaveWithoutPayPercentage,
                GETUTCDATE() AS CreatedAt
            FROM ABSMIS
            WHERE UNTID = @UnitId AND MNTH = @Month
            """;

        await using var connection = new SqlConnection(connectionString);
        var command = new CommandDefinition(sql,
            new { UnitId = unitId, Month = month },
            cancellationToken: cancellationToken);

        return await connection.QueryAsync<AbsenteeismMisDto>(command);
    }
}
