using Dapper;

namespace AttendanceService.Infrastructure.Dapper;

public class AttendanceDapperRepository(DapperContext context)
{
    public async Task<decimal> GetAttendancePercentageAsync(long empSysId, DateTime from, DateTime to)
    {
        const string sql = """
            SELECT ISNULL(
                (COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE)) * 100.0) / NULLIF(DATEDIFF(DAY, @from, @to) + 1, 0)
            , 0)
            FROM SWIPE_RAWPUNCH
            WHERE SWIPE_EMPSYSID = @empSysId
              AND SWIPE_PUNCHTIME >= @from
              AND SWIPE_PUNCHTIME <= @to
            """;

        using var conn = context.CreateConnection();
        return await conn.ExecuteScalarAsync<decimal>(sql, new { empSysId, from, to });
    }

    public async Task<int> GetLopDaysAsync(long empSysId, DateTime from, DateTime to)
    {
        const string sql = """
            SELECT ISNULL(DATEDIFF(DAY, @from, @to) + 1
                - COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE)), 0)
            FROM SWIPE_RAWPUNCH
            WHERE SWIPE_EMPSYSID = @empSysId
              AND SWIPE_PUNCHTIME >= @from
              AND SWIPE_PUNCHTIME <= @to
            """;

        using var conn = context.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sql, new { empSysId, from, to });
    }
}
