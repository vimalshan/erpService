using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace TravelService.Functions;

/// <summary>
/// Timer-triggered Azure Function to process pending travel batches.
/// Runs every day at 00:30 UTC.
/// </summary>
public class BatchProcessingFunction
{
    private readonly ILogger<BatchProcessingFunction> _logger;
    private readonly string _connectionString;

    public BatchProcessingFunction(ILogger<BatchProcessingFunction> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("TravelDb")
            ?? throw new InvalidOperationException("TravelDb connection string not configured.");
    }

    [Function("BatchProcessingFunction")]
    public async Task RunAsync([TimerTrigger("0 30 0 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("BatchProcessingFunction started at {Time}", DateTime.UtcNow);

        try
        {
            using var connection = new SqlConnection(_connectionString);
            var pendingBatches = await connection.QueryAsync<string>(
                "SELECT BATCH_ID FROM TRAVEL_BATCHMAIN WHERE BATCH_STATUS = 'P' AND BATCH_CREATEDON < DATEADD(day, -1, GETUTCDATE())");

            var batchIds = pendingBatches.ToList();
            _logger.LogInformation("Found {Count} pending batches to process.", batchIds.Count);

            foreach (var batchId in batchIds)
            {
                _logger.LogInformation("Processing batch {BatchId}", batchId);
                // Batch processing logic would delegate to application services
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during batch processing.");
            throw;
        }

        _logger.LogInformation("BatchProcessingFunction completed at {Time}", DateTime.UtcNow);
    }
}
