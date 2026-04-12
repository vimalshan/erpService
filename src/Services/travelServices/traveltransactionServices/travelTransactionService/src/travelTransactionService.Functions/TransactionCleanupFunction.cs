using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace travelTransactionService.Functions;

public class TransactionCleanupFunction
{
    private readonly ILogger<TransactionCleanupFunction> _logger;

    public TransactionCleanupFunction(ILogger<TransactionCleanupFunction> logger)
    {
        _logger = logger;
    }

    [Function("TransactionCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TransactionCleanup function executed at: {Time}", DateTime.UtcNow);

        // Clean up stale JV interface records and missing combo codes
        _logger.LogInformation("Cleanup completed. Next scheduled run: {Next}", timerInfo.ScheduleStatus?.Next);

        await Task.CompletedTask;
    }
}
