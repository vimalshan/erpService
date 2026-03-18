using DealTicketing.Application.Features.DealDetails.Queries;
using DealTicketing.Infrastructure.Persistence;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DealTicketing.Functions;

/// <summary>
/// Timer-triggered function that runs daily at 8 AM UTC
/// to notify approvers about deals expiring within 7 days.
/// </summary>
public class DealExpiryNotificationFunction(
    DealTicketingDbContext dbContext,
    ILogger<DealExpiryNotificationFunction> logger)
{
    [Function(nameof(DealExpiryNotificationFunction))]
    public async Task Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        logger.LogInformation("DealExpiryNotificationFunction triggered at {Time}", DateTime.UtcNow);

        var cutoff = DateTime.UtcNow.AddDays(7);
        var expiringDeals = await dbContext.DealDetails
            .Where(d => d.DealMatDate.HasValue
                        && d.DealMatDate.Value <= cutoff
                        && d.DealSetStatus == 'L')
            .Select(d => new { d.DealId, d.DealMatDate, d.DealAmount, d.DealBatchId })
            .ToListAsync(ct);

        if (expiringDeals.Count == 0)
        {
            logger.LogInformation("No deals expiring within 7 days.");
            return;
        }

        logger.LogWarning(
            "Found {Count} deals expiring within 7 days. IDs: {Ids}",
            expiringDeals.Count,
            string.Join(", ", expiringDeals.Select(d => d.DealId)));

        // TODO: integrate with email/notification service
        foreach (var deal in expiringDeals)
        {
            logger.LogInformation(
                "Expiring Deal: ID={DealId}, BatchId={BatchId}, Amount={Amount}, MatDate={MatDate}",
                deal.DealId, deal.DealBatchId, deal.DealAmount, deal.DealMatDate);
        }
    }
}
