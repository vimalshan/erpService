using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VisitorServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace VisitorServices.Functions.Functions;

/// <summary>
/// Runs nightly to auto-checkout visitors who checked in but never checked out (stale records).
/// </summary>
public class VisitorCleanupFunction(ILogger<VisitorCleanupFunction> logger, VisitorDbContext dbContext)
{
    [Function("VisitorCleanup")]
    public async Task Run(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo,  // 2 AM daily
        CancellationToken cancellationToken)
    {
        logger.LogInformation("VisitorCleanup triggered at {Time}", DateTime.UtcNow);

        var cutoff = DateTime.UtcNow.AddHours(-24);

        var staleVisitors = await dbContext.Visitors
            .Where(v => (char)(int)v.Status == 'I' && v.CheckInTime < cutoff)
            .ToListAsync(cancellationToken);

        if (staleVisitors.Count == 0)
        {
            logger.LogInformation("No stale visitors found.");
            return;
        }

        foreach (var visitor in staleVisitors)
            visitor.Checkout(0); // System checkout (userId = 0)

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Auto-checked-out {Count} stale visitor(s).", staleVisitors.Count);
    }
}
