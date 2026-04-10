using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SSCTransactional.Functions;

public class AllocationCleanupFunction
{
    private readonly ILogger<AllocationCleanupFunction> _logger;

    public AllocationCleanupFunction(ILogger<AllocationCleanupFunction> logger)
        => _logger = logger;

    /// <summary>
    /// Runs daily at 02:00 UTC to archive old completed/rejected allocations.
    /// </summary>
    [Function("AllocationCleanup")]
    public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[Function] AllocationCleanup triggered at: {Time}", DateTime.UtcNow);
        // TODO: Query and archive allocations older than 90 days with completed/rejected status
        _logger.LogInformation("[Function] AllocationCleanup completed.");
    }
}
