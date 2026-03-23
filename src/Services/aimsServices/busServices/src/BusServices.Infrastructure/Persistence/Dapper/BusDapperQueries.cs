using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BusServices.Infrastructure.Persistence.Dapper;

public sealed class BusDapperQueries
{
    private readonly string _connectionString;

    public BusDapperQueries(IConfiguration config)
        => _connectionString = config.GetConnectionString("BusDb")
            ?? throw new InvalidOperationException("BusDb connection string not configured.");

    public async Task<IEnumerable<BusSummaryResult>> GetBusSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                b.BUS_ID           AS BusId,
                b.BUS_REGNUM       AS RegistrationNumber,
                b.BUS_DESCRIPTION  AS Description,
                b.BUS_CAPACITY     AS Capacity,
                COUNT(DISTINCT eb.EMPBUS_ID) AS AssignedEmployees,
                COUNT(DISTINCT br.ROUTE_ID)  AS TotalRoutes
            FROM BUS_MASTER b
            LEFT JOIN EMPLOYEE_BUS eb ON eb.EMPBUS_BUSID = b.BUS_ID AND eb.EMPBUS_CLSDATE IS NULL
            LEFT JOIN BUSROUTE_MASTER br ON br.ROUTE_BUS_ID = b.BUS_ID
            GROUP BY b.BUS_ID, b.BUS_REGNUM, b.BUS_DESCRIPTION, b.BUS_CAPACITY
            ORDER BY b.BUS_ID;
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<BusSummaryResult>(sql);
    }

    public async Task<IEnumerable<ArrivalReportResult>> GetArrivalReportAsync(DateTime fromDate, DateTime toDate, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                a.ARRIVAL_ID        AS ArrivalId,
                b.BUS_REGNUM        AS RegistrationNumber,
                a.ARRIVAL_DATE      AS ArrivalDate,
                a.ARRIVAL_TIME      AS ArrivalTime,
                a.ARRIVAL_STATUS    AS Status,
                a.ARRIVAL_REMARKS   AS Remarks
            FROM BUS_ARRIVALDET a
            INNER JOIN BUS_MASTER b ON b.BUS_ID = a.ARRIVAL_BUS_ID
            WHERE a.ARRIVAL_DATE BETWEEN @FromDate AND @ToDate
            ORDER BY a.ARRIVAL_DATE DESC, a.ARRIVAL_TIME DESC;
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<ArrivalReportResult>(sql, new { FromDate = fromDate.Date, ToDate = toDate.Date });
    }

    public async Task<IEnumerable<EmployeeBusReportResult>> GetEmployeeBusReportAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                eb.EMPBUS_ID        AS EmpBusId,
                eb.EMPBUS_EMPSYSID  AS EmpSysId,
                b.BUS_REGNUM        AS RegistrationNumber,
                r.ROUTE_NAME        AS RouteName,
                eb.EMPBUS_EFFDATE   AS EffectiveDate,
                eb.EMPBUS_CLSDATE   AS ClosingDate
            FROM EMPLOYEE_BUS eb
            INNER JOIN BUS_MASTER b   ON b.BUS_ID   = eb.EMPBUS_BUSID
            INNER JOIN BUSROUTE_MASTER r ON r.ROUTE_ID = eb.EMPBUS_ROUTEID
            ORDER BY eb.EMPBUS_EMPSYSID, eb.EMPBUS_EFFDATE DESC;
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<EmployeeBusReportResult>(sql);
    }
}

public record BusSummaryResult(int BusId, string RegistrationNumber, string? Description, int Capacity, int AssignedEmployees, int TotalRoutes);
public record ArrivalReportResult(long ArrivalId, string RegistrationNumber, DateTime ArrivalDate, TimeSpan ArrivalTime, string Status, string? Remarks);
public record EmployeeBusReportResult(long EmpBusId, long EmpSysId, string RegistrationNumber, string RouteName, DateTime EffectiveDate, DateTime? ClosingDate);
