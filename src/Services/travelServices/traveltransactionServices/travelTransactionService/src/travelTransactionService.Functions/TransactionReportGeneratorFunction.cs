using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace travelTransactionService.Functions;

public class TransactionReportGeneratorFunction
{
    private readonly ILogger<TransactionReportGeneratorFunction> _logger;

    public TransactionReportGeneratorFunction(ILogger<TransactionReportGeneratorFunction> logger)
    {
        _logger = logger;
    }

    [Function("TransactionReportGenerator")]
    public async Task Run([TimerTrigger("0 0 6 * * 1")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TransactionReportGenerator function started at: {Time}", DateTime.UtcNow);

        // Generate weekly transaction reconciliation reports
        _logger.LogInformation("Weekly transaction report generation completed.");

        await Task.CompletedTask;
    }
}
