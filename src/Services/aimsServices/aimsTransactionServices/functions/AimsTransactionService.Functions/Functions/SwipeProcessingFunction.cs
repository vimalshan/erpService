using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AimsTransactionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Domain.Enums;

namespace AimsTransactionService.Functions.Functions;

/// <summary>
/// Runs every hour to process unprocessed swipe punches and flag anomalies.
/// </summary>
public class SwipeProcessingFunction(ILogger<SwipeProcessingFunction> logger, AimsTransactionDbContext dbContext)
{
    [Function("SwipeProcessing")]
    public async Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo timerInfo,  // every hour
        CancellationToken cancellationToken)
    {
        logger.LogInformation("SwipeProcessing triggered at {Time}", DateTime.UtcNow);

        var threshold = DateTime.UtcNow.AddHours(-2);

        var unprocessedSwipes = await dbContext.Swipes
            .Where(s => s.PullStatus == PullStatus.Automatic && s.UpdatedOn < threshold)
            .ToListAsync(cancellationToken);

        foreach (var swipe in unprocessedSwipes)
        {
            // Integration point: validate swipe pairs (In/Out), flag anomalies
            logger.LogInformation(
                "Processing swipe {SwipeId} for employee {EmployeeSysId} at {PunchTime}",
                swipe.Id, swipe.EmployeeSysId, swipe.PunchTime);
        }

        logger.LogInformation("Processed {Count} swipe record(s).", unprocessedSwipes.Count);
    }
}
