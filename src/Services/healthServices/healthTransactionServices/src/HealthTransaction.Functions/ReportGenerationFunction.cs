using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HealthTransaction.Functions;

/// <summary>
/// Runs weekly on Sunday at 06:00 — generates health summary reports.
/// </summary>
public class ReportGenerationFunction
{
    private readonly SqlConnection _connection;
    private readonly ILogger<ReportGenerationFunction> _logger;

    public ReportGenerationFunction(SqlConnection connection, ILogger<ReportGenerationFunction> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    [Function("ReportGeneration")]
    public async Task Run([TimerTrigger("0 0 6 * * 0")] TimerInfo myTimer)
    {
        _logger.LogInformation("ReportGeneration triggered at {Time}", DateTime.UtcNow);

        var summary = await _connection.QueryAsync<dynamic>(
            @"SELECT CPM_COM_COD AS CompanyCode, COUNT(*) AS CheckupCount,
                     COUNT(CASE WHEN CPM_FIT_FINAL = 'Y' THEN 1 END) AS FitCount
              FROM CHKUP_PRE_MAIN
              WHERE CPM_CHK_DAT >= @WeekStart
              GROUP BY CPM_COM_COD",
            new { WeekStart = DateTime.Today.AddDays(-7) });

        _logger.LogInformation("Weekly Health Report:\n{Report}", JsonSerializer.Serialize(summary));
    }
}
