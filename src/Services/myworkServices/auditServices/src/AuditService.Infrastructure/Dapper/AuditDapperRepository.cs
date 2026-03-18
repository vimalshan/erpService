using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace AuditService.Infrastructure.Dapper;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public SqlConnection CreateConnection() => new(_connectionString);
}

public class AuditDapperRepository
{
    private readonly DapperContext _context;

    public AuditDapperRepository(DapperContext context) => _context = context;

    public async Task<IEnumerable<dynamic>> GetAuditSummaryAsync(int year, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT 
                am.AUDIT_ID, am.AUDIT_NAME, am.AUDIT_STATUS, am.AUDIT_UNIT,
                COUNT(ao.OBV_ID) AS TotalObservations,
                SUM(CASE WHEN ao.OBV_STATUS = 'C' THEN 1 ELSE 0 END) AS CompletedObservations,
                SUM(CASE WHEN ao.OBV_STATUS = 'P' THEN 1 ELSE 0 END) AS PendingObservations
            FROM AUDIT_MASTER am
            LEFT JOIN AUDIT_OBSERVATION ao ON am.AUDIT_ID = ao.OBV_AUDITID
            WHERE YEAR(am.AUDIT_PLANFROM) = @Year
            GROUP BY am.AUDIT_ID, am.AUDIT_NAME, am.AUDIT_STATUS, am.AUDIT_UNIT";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync(sql, new { Year = year });
    }

    public async Task<IEnumerable<dynamic>> GetOverdueObservationsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT 
                ao.OBV_ID, ao.OBV_TITLE, ao.OBV_RISK, ao.OBV_ORGDUEDATE,
                ao.OBV_AUDITEE, am.AUDIT_NAME, am.AUDIT_UNIT,
                DATEDIFF(DAY, ao.OBV_ORGDUEDATE, GETDATE()) AS DaysOverdue
            FROM AUDIT_OBSERVATION ao
            INNER JOIN AUDIT_MASTER am ON ao.OBV_AUDITID = am.AUDIT_ID
            WHERE ao.OBV_STATUS = 'P' AND ao.OBV_ORGDUEDATE < GETDATE()
            ORDER BY ao.OBV_ORGDUEDATE ASC";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync(sql);
    }

    public async Task<IEnumerable<dynamic>> GetTopRatedPracticesAsync(int topN = 10, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT TOP (@TopN)
                gp.PRACTICE_ID, gp.PRACTICE_TITLE, gp.PRACTICE_UNIT,
                AVG(CAST(r.PRACTICE_RATING AS FLOAT)) AS AvgRating,
                COUNT(r.PRACTICE_RATINGID) AS RatingCount
            FROM AUDIT_GOODPRACTICE gp
            LEFT JOIN AUDIT_GOODPRACTICERATING r ON gp.PRACTICE_ID = r.PRACTICE_ID
            GROUP BY gp.PRACTICE_ID, gp.PRACTICE_TITLE, gp.PRACTICE_UNIT
            HAVING COUNT(r.PRACTICE_RATINGID) > 0
            ORDER BY AvgRating DESC";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync(sql, new { TopN = topN });
    }
}
