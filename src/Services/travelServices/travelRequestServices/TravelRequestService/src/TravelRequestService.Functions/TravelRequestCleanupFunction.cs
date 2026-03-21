using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TravelRequestService.Functions;

public class TravelRequestCleanupFunction
{
    private readonly ILogger<TravelRequestCleanupFunction> _logger;

    public TravelRequestCleanupFunction(ILogger<TravelRequestCleanupFunction> logger)
    {
        _logger = logger;
    }

    [Function("TravelRequestCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TravelRequestCleanup function executed at: {Time}", DateTime.UtcNow);

        // Clean up expired/stale travel requests
        // In production, inject IUnitOfWork and perform cleanup
        _logger.LogInformation("Cleanup completed. Next scheduled run: {Next}", timerInfo.ScheduleStatus?.Next);

        await Task.CompletedTask;
    }
}
