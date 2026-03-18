using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RequestServices.Infrastructure.Data;

namespace RequestServices.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs every day at midnight UTC.
/// Purges cancelled requests older than 90 days to keep the database lean.
/// </summary>
public class PurgeCancelledRequestsFunction(
    RequestDbContext context,
    ILogger<PurgeCancelledRequestsFunction> logger)
{
    // Runs at 00:00 UTC every day: "0 0 0 * * *"
    [Function(nameof(PurgeCancelledRequestsFunction))]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("PurgeCancelledRequests triggered at {Now}", DateTime.UtcNow);

        var cutoff = DateTime.UtcNow.AddDays(-90);

        var staleSubRequests = await context.RequestSub
            .Where(s => s.StatusCode == 'C'
                     && s.CancellationDate.HasValue
                     && s.CancellationDate.Value < cutoff)
            .ToListAsync();

        if (staleSubRequests.Count == 0)
        {
            logger.LogInformation("No cancelled requests to purge.");
            return;
        }

        context.RequestSub.RemoveRange(staleSubRequests);
        var deleted = await context.SaveChangesAsync();
        logger.LogInformation("Purged {Count} stale cancelled sub-requests.", deleted);
    }
}
