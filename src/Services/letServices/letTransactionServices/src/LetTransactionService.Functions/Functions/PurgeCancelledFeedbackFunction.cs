using LetTransactionService.Infrastructure.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs every day at midnight UTC.
/// Archives cancelled feedback records older than 90 days.
/// </summary>
public class PurgeCancelledFeedbackFunction(
    LetTransactionDbContext context,
    ILogger<PurgeCancelledFeedbackFunction> logger)
{
    [Function(nameof(PurgeCancelledFeedbackFunction))]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("PurgeCancelledFeedback triggered at {Now}", DateTime.UtcNow);

        var cutoff = DateTime.UtcNow.AddDays(-90);

        var staleFeedback = await context.CourseFeedbackMain
            .Where(f => f.StatusCode == 'X' && f.FeedbackDate < cutoff)
            .ToListAsync();

        if (staleFeedback.Count == 0)
        {
            logger.LogInformation("No cancelled feedback to purge.");
            return;
        }

        context.CourseFeedbackMain.RemoveRange(staleFeedback);
        var deleted = await context.SaveChangesAsync();
        logger.LogInformation("Purged {Count} stale cancelled feedback records.", deleted);
    }
}
