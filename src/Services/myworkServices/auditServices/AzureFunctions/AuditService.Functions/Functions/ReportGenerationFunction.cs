using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuditService.Functions.Functions;

/// <summary>
/// Runs monthly on the 1st at 06:00 UTC to generate audit summary reports.
/// </summary>
public class ReportGenerationFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReportGenerationFunction> _logger;

    public ReportGenerationFunction(IConfiguration configuration, ILogger<ReportGenerationFunction> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [Function(nameof(ReportGenerationFunction))]
    public async Task Run([TimerTrigger("0 0 6 1 * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ReportGenerationFunction triggered at {UtcNow}", DateTime.UtcNow);

        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

        const string sql = @"
            SELECT
                am.AUDIT_ID, am.AUDIT_NAME, am.AUDIT_STATUS, am.AUDIT_UNIT,
                COUNT(ao.OBV_ID) AS TotalObservations,
                SUM(CASE WHEN ao.OBV_STATUS = 'C' THEN 1 ELSE 0 END) AS CompletedObservations,
                SUM(CASE WHEN ao.OBV_STATUS = 'P' THEN 1 ELSE 0 END) AS PendingObservations,
                SUM(CASE WHEN ao.OBV_RISK = 'A' THEN 1 ELSE 0 END) AS HighRiskObservations,
                AVG(DATEDIFF(DAY, ao.OBV_ORGDUEDATE, COALESCE(ao.OBV_COMPLETEDON, GETDATE()))) AS AvgResolutionDays
            FROM AUDIT_MASTER am
            LEFT JOIN AUDIT_OBSERVATION ao ON am.AUDIT_ID = ao.OBV_AUDITID
            WHERE YEAR(am.AUDIT_PLANFROM) = YEAR(GETDATE())
            GROUP BY am.AUDIT_ID, am.AUDIT_NAME, am.AUDIT_STATUS, am.AUDIT_UNIT
            ORDER BY HighRiskObservations DESC, PendingObservations DESC";

        using var connection = new SqlConnection(connectionString);
        var report = (await connection.QueryAsync(sql)).ToList();

        _logger.LogInformation("Monthly report generated: {AuditCount} audits, {TotalObservations} total observations.",
            report.Count, report.Sum(r => (int)((r.TotalObservations as int?) ?? 0)));

        foreach (var row in report)
        {
            long auditId = (long)(row.AUDIT_ID ?? 0L);
            string auditName = (string)(row.AUDIT_NAME ?? string.Empty);
            int total = (int)(row.TotalObservations ?? 0);
            int completed = (int)(row.CompletedObservations ?? 0);
            int pending = (int)(row.PendingObservations ?? 0);
            int highRisk = (int)(row.HighRiskObservations ?? 0);

            _logger.LogInformation(
                "Audit {AuditId} ({AuditName}): Total={Total}, Completed={Completed}, Pending={Pending}, HighRisk={HighRisk}",
                auditId, auditName, total, completed, pending, highRisk);
        }
    }
}
