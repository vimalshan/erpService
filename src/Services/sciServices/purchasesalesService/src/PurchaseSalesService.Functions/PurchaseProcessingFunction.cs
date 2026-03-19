using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseSalesService.Functions;

/// <summary>
/// Background function that runs every 5 minutes to process pending purchases.
/// </summary>
public sealed class PurchaseProcessingFunction
{
    private readonly ILogger<PurchaseProcessingFunction> _logger;

    public PurchaseProcessingFunction(ILogger<PurchaseProcessingFunction> logger)
        => _logger = logger;

    [Function("ProcessPendingPurchases")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("ProcessPendingPurchases triggered at {Time}", DateTime.UtcNow);

        // TODO: Query DB for unprocessed purchases (PD_CAN_FLG IS NULL / stage=1)
        // and advance their workflow stage.
    }
}
