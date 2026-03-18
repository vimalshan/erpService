using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CalendarService.Infrastructure.Dapper;

public record ShiftSummaryRow(int ShiftId, string ShiftCode, string ShiftName, decimal Duration, int ExceptionCount);
public record HolidaySummaryRow(int HolidayId, string HolidayDate, string Description, string Type);
public record CalendarSummaryRow(int CalendarId, string CalendarName, string Status, int UnitCount);

public class DapperReadService(IConfiguration config)
{
    private SqlConnection CreateConnection()
        => new(config.GetConnectionString("CalendarDb"));

    public async Task<IEnumerable<ShiftSummaryRow>> GetShiftSummariesAsync()
    {
        const string sql = """
            SELECT s.SHIFT_ID          AS ShiftId,
                   s.SHIFT_CODE        AS ShiftCode,
                   s.SHIFT_NAME        AS ShiftName,
                   s.SHIFT_DURATION    AS Duration,
                   COUNT(e.SHIFTEXC_ID) AS ExceptionCount
            FROM   SHIFT_MASTER s
            LEFT JOIN SHIFT_EXCEPTION e ON e.SHIFTEXC_SHIFTID = s.SHIFT_ID
            GROUP BY s.SHIFT_ID, s.SHIFT_CODE, s.SHIFT_NAME, s.SHIFT_DURATION
            ORDER BY s.SHIFT_CODE
            """;
        using var conn = CreateConnection();
        return await conn.QueryAsync<ShiftSummaryRow>(sql);
    }

    public async Task<IEnumerable<HolidaySummaryRow>> GetUpcomingHolidaysAsync(int days = 30)
    {
        const string sql = """
            SELECT HOLIDAY_ID          AS HolidayId,
                   CONVERT(VARCHAR,HOLIDAY_DATE,103) AS HolidayDate,
                   HOLIDAY_DESCRIPTION AS Description,
                   HOLIDAY_TYPE        AS Type
            FROM   HOLIDAY_MASTER
            WHERE  HOLIDAY_DATE BETWEEN GETDATE() AND DATEADD(DAY,@Days,GETDATE())
            ORDER BY HOLIDAY_DATE
            """;
        using var conn = CreateConnection();
        return await conn.QueryAsync<HolidaySummaryRow>(sql, new { Days = days });
    }

    public async Task<IEnumerable<CalendarSummaryRow>> GetCalendarSummariesAsync()
    {
        const string sql = """
            SELECT c.CALENDAR_ID   AS CalendarId,
                   c.CALENDAR_NAME AS CalendarName,
                   c.CALENDAR_STATUS AS Status,
                   COUNT(u.CALUNIT_ID) AS UnitCount
            FROM   CALENDAR_MASTER c
            LEFT JOIN CALENDAR_UNITMAP u ON u.CALUNIT_CALENID = c.CALENDAR_ID
            GROUP BY c.CALENDAR_ID, c.CALENDAR_NAME, c.CALENDAR_STATUS
            ORDER BY c.CALENDAR_NAME
            """;
        using var conn = CreateConnection();
        return await conn.QueryAsync<CalendarSummaryRow>(sql);
    }
}
