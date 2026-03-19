using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BatchAndEnvelopeService.Functions;

public class BatchCleanupFunction
{
    private readonly ILogger<BatchCleanupFunction> _logger;

    public BatchCleanupFunction(ILogger<BatchCleanupFunction> logger)
        => _logger = logger;

    /// <summary>
    /// Runs daily at 02:00 UTC to archive old cancelled batches.
    /// </summary>
    [Function("BatchCleanup")]
    public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[Function] BatchCleanup triggered at: {Time}", DateTime.UtcNow);
        // TODO: Query and archive batches older than 90 days with cancelled status
        _logger.LogInformation("[Function] BatchCleanup completed.");
    }
}
