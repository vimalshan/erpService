using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CardManagement.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs daily to process expired cards.
/// </summary>
public class ExpiredCardProcessorFunction
{
    private readonly ILogger<ExpiredCardProcessorFunction> _logger;

    public ExpiredCardProcessorFunction(ILogger<ExpiredCardProcessorFunction> logger)
        => _logger = logger;

    [Function("ExpiredCardProcessor")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("ExpiredCardProcessor running at {Time}", DateTime.UtcNow);

        // TODO: Inject IMediator/service and close expired cards
        await Task.CompletedTask;

        _logger.LogInformation("ExpiredCardProcessor completed.");
    }
}
