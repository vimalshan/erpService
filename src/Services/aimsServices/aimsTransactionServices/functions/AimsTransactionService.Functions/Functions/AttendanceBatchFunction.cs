using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AimsTransactionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Domain.Enums;

namespace AimsTransactionService.Functions.Functions;

/// <summary>
/// Runs nightly to process monthly attendance batches that are still in 'Processing' status.
/// </summary>
public class AttendanceBatchFunction(ILogger<AttendanceBatchFunction> logger, AimsTransactionDbContext dbContext)
{
    [Function("AttendanceBatchProcessing")]
    public async Task Run(
        [TimerTrigger("0 0 3 * * *")] TimerInfo timerInfo,  // 3 AM daily
        CancellationToken cancellationToken)
    {
        logger.LogInformation("AttendanceBatchProcessing triggered at {Time}", DateTime.UtcNow);

        var staleBatches = await dbContext.AttendanceBatches
            .Where(b => b.Status == BatchStatus.Processing)
            .ToListAsync(cancellationToken);

        if (staleBatches.Count == 0)
        {
            logger.LogInformation("No stale attendance batches found.");
            return;
        }

        foreach (var batch in staleBatches)
        {
            // Integration point: complete batch processing, calculate LOP, overtime, summaries
            logger.LogWarning(
                "Attendance batch {BatchId} (month {MonthStart:yyyy-MM}) is still processing since {CreatedOn}",
                batch.Id, batch.MonthStart, batch.CreatedOn);
        }

        logger.LogInformation("Found {Count} stale attendance batch(es).", staleBatches.Count);
    }
}
