using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocumentService.Functions.Functions;

/// <summary>
/// Runs daily at 02:00 UTC to purge orphaned document records older than 90 days
/// where the associated loan no longer exists (soft cleanup).
/// </summary>
public class DocumentCleanupFunction
{
    private readonly ILogger<DocumentCleanupFunction> _logger;
    private readonly string _connectionString;

    public DocumentCleanupFunction(ILogger<DocumentCleanupFunction> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration["SqlConnectionString"]
            ?? throw new InvalidOperationException("SqlConnectionString is not configured.");
    }

    [Function("DocumentCleanup")]
    public async Task Run(
        [TimerTrigger("%DocumentCleanupSchedule%")] TimerInfo timer,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("DocumentCleanup triggered at {Time}", DateTimeOffset.UtcNow);

        try
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = """
                DELETE FROM LOAN_DOCUMENTS
                WHERE LOANDOC_LASTMODIFIEDON < DATEADD(DAY, -90, GETUTCDATE())
                """;

            var affected = await connection.ExecuteAsync(new CommandDefinition(sql, cancellationToken: cancellationToken));
            _logger.LogInformation("DocumentCleanup: removed {Count} stale record(s).", affected);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DocumentCleanup failed.");
            throw;
        }

        if (timer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next DocumentCleanup scheduled at: {Next}", timer.ScheduleStatus.Next);
        }
    }
}
