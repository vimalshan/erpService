using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderService.Functions;

public class PurchaseOrderCleanupFunction
{
    private readonly ILogger<PurchaseOrderCleanupFunction> _logger;

    public PurchaseOrderCleanupFunction(ILogger<PurchaseOrderCleanupFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Runs daily at midnight to clean up stale DRAFT purchase orders older than 30 days.
    /// </summary>
    [Function("PurchaseOrderCleanup")]
    public async Task RunCleanup([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("PurchaseOrder Cleanup function started at: {Time}", DateTime.UtcNow);

        // In production, inject IPurchaseOrderRepository and clean up stale drafts
        // var staleDrafts = await repository.GetStaleDraftsAsync(DateTime.UtcNow.AddDays(-30));
        // foreach (var draft in staleDrafts) { draft.Cancel(); }

        _logger.LogInformation("PurchaseOrder Cleanup function completed at: {Time}", DateTime.UtcNow);
        await Task.CompletedTask;
    }
}
