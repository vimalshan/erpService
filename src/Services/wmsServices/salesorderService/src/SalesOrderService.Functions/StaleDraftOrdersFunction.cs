using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;
using SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;

namespace SalesOrderService.Functions;

/// <summary>
/// Timer-triggered function that runs nightly to check for stale draft orders
/// and logs a summary. In production this could escalate or auto-cancel them.
/// </summary>
public sealed class StaleDraftOrdersFunction(
    ILogger<StaleDraftOrdersFunction> logger,
    ISender mediator)
{
    // Runs every day at 02:00 UTC
    [Function(nameof(StaleDraftOrdersFunction))]
    public async Task Run(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("StaleDraftOrders function triggered at: {Time}", DateTime.UtcNow);

        var orders = await mediator.Send(new GetAllSalesOrdersQuery(), cancellationToken);

        var staleDrafts = orders
            .Where(o => o.Status == "DRAFT" &&
                        o.OrderDate < DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)))
            .ToList();

        if (staleDrafts.Count == 0)
        {
            logger.LogInformation("No stale draft orders found.");
            return;
        }

        logger.LogWarning(
            "Found {Count} draft orders older than 7 days: {Orders}",
            staleDrafts.Count,
            string.Join(", ", staleDrafts.Select(o => o.SoNumber)));
    }
}
