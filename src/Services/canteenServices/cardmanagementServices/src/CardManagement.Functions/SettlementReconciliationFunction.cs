using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CardManagement.Functions;

/// <summary>
/// Timer-triggered function for daily card settlement reconciliation.
/// </summary>
public class SettlementReconciliationFunction
{
    private readonly ILogger<SettlementReconciliationFunction> _logger;

    public SettlementReconciliationFunction(ILogger<SettlementReconciliationFunction> logger)
        => _logger = logger;

    [Function("SettlementReconciliation")]
    public async Task Run([TimerTrigger("0 30 1 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("SettlementReconciliation running at {Time}", DateTime.UtcNow);

        // TODO: Inject settlement service and reconcile pending settlements
        await Task.CompletedTask;

        _logger.LogInformation("SettlementReconciliation completed.");
    }
}
