using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Infrastructure.Services;

/// <summary>
/// Dapper-based read service for high-performance reporting queries
/// that bypass EF tracking overhead.
/// </summary>
public class VisitorReadService(IConfiguration configuration)
{
    private SqlConnection CreateConnection() =>
        new(configuration.GetConnectionString("VisitorDb"));

    public async Task<IEnumerable<VisitorDto>> GetVisitorsByDateRangeAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        await using var conn = CreateConnection();
        const string sql = """
            SELECT
                VISITOR_ID        AS VisitorId,
                VISITOR_NAME      AS VisitorName,
                VISITOR_IDTYPE    AS IdType,
                VISITOR_IDNUMBER  AS IdNumber,
                VISITOR_PHONENUMBER AS PhoneNumber,
                VISITOR_EMAIL     AS Email,
                VISITOR_COMPANY   AS Company,
                VISITOR_PURPOSE   AS Purpose,
                VISITOR_CHECKINTIME  AS CheckInTime,
                VISITOR_CHECKOUTTIME AS CheckOutTime,
                VISITOR_STATUS    AS Status,
                VISITOR_WHOMTOVISIT AS WhomToVisit,
                VISITOR_ENTEREDON AS EnteredOn,
                VISITOR_ENTEREDBY AS EnteredBy
            FROM VISITOR_MAIN
            WHERE VISITOR_CHECKINTIME BETWEEN @From AND @To
            ORDER BY VISITOR_CHECKINTIME DESC
        """;

        var rows = await conn.QueryAsync<dynamic>(
            sql,
            new { From = from, To = to },
            commandTimeout: 30);

        return rows.Select(r => new VisitorDto(
            (long)r.VisitorId, (string)r.VisitorName,
            (string)r.IdType, (string?)r.IdNumber,
            (string?)r.PhoneNumber, (string?)r.Email,
            (string?)r.Company, (string?)r.Purpose,
            (DateTime)r.CheckInTime, (DateTime?)r.CheckOutTime,
            (string)r.Status, (long)r.WhomToVisit,
            (DateTime)r.EnteredOn, (long)r.EnteredBy));
    }

    public async Task<int> GetCurrentOccupancyAsync(CancellationToken cancellationToken = default)
    {
        await using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM VISITOR_MAIN WHERE VISITOR_STATUS = 'I'");
    }
}
