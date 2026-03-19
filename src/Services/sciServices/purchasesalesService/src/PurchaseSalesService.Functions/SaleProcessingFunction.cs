using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseSalesService.Functions;

/// <summary>
/// Background function that runs every 10 minutes to process pending sales.
/// </summary>
public sealed class SaleProcessingFunction
{
    private readonly ILogger<SaleProcessingFunction> _logger;

    public SaleProcessingFunction(ILogger<SaleProcessingFunction> logger)
        => _logger = logger;

    [Function("ProcessPendingSales")]
    public void Run([TimerTrigger("0 */10 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("ProcessPendingSales triggered at {Time}", DateTime.UtcNow);

        // TODO: Query DB for pending sales (SL_CAN_FLG IS NULL / stage=1)
        // generate invoices, update ISO info, notify downstream services.
    }
}
