using EmployeeTransactionsService.Application.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmployeeTransactionsService.Functions.Functions;

public sealed class ProbationReminderFunction(ITransactionReadService transactionReadService, ILogger<ProbationReminderFunction> logger)
{
    [Function("ProbationReminderFunction")]
    public async Task Run([TimerTrigger("0 0 7 * * *")] TimerInfo timerInfo)
    {
        var timeline = await transactionReadService.GetEmployeeTimelineAsync(1, CancellationToken.None);
        logger.LogInformation("ProbationReminderFunction executed. Timeline sample count: {Count}", timeline.Count);
    }
}

public sealed class AlertGroupAuditFunction(ILogger<AlertGroupAuditFunction> logger)
{
    [Function("AlertGroupAuditFunction")]
    public Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("AlertGroupAuditFunction executed at {Time}", DateTime.UtcNow);
        return Task.CompletedTask;
    }
}

public sealed class StationeryImageCleanupFunction(ILogger<StationeryImageCleanupFunction> logger)
{
    [Function("StationeryImageCleanupFunction")]
    public Task Run([TimerTrigger("0 30 2 * * 0")] TimerInfo timerInfo)
    {
        logger.LogInformation("StationeryImageCleanupFunction executed at {Time}", DateTime.UtcNow);
        return Task.CompletedTask;
    }
}