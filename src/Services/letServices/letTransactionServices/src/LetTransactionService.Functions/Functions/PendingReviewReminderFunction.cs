using LetTransactionService.Infrastructure.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function that sends reminder notifications
/// for pending LET reviews older than 7 days.
/// Runs every morning at 07:00 UTC.
/// </summary>
public class PendingReviewReminderFunction(
    LetTransactionDbContext context,
    ILogger<PendingReviewReminderFunction> logger)
{
    [Function(nameof(PendingReviewReminderFunction))]
    public async Task Run([TimerTrigger("0 0 7 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("PendingReviewReminder triggered at {Now}", DateTime.UtcNow);

        var threshold = DateTime.UtcNow.AddDays(-7);

        var staleReviews = await context.ReviewMain
            .Where(r => r.Status == 'P' && r.NextReviewDate < threshold)
            .ToListAsync();

        if (staleReviews.Count == 0)
        {
            logger.LogInformation("No pending reviews requiring reminders.");
            return;
        }

        foreach (var review in staleReviews)
        {
            logger.LogWarning(
                "REMINDER: Review {ReviewSerialNumber} (feedback {FeedbackNumber}) has been pending since {Date}.",
                review.ReviewSerialNumber, review.FeedbackNumber, review.NextReviewDate);
        }

        logger.LogInformation("Sent reminders for {Count} stale reviews.", staleReviews.Count);
    }
}
