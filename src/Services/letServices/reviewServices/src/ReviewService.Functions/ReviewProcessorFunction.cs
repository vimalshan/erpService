using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ReviewService.Functions;

/// <summary>
/// Timer-triggered Azure Function for background review processing.
/// Runs every hour on the hour.
/// </summary>
public class ReviewProcessorFunction
{
    private readonly ILogger<ReviewProcessorFunction> _logger;

    public ReviewProcessorFunction(ILogger<ReviewProcessorFunction> logger)
        => _logger = logger;

    [Function("ReviewSummaryProcessor")]
    public async Task RunAsync(
        [TimerTrigger("0 0 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation(
            "ReviewSummaryProcessor triggered at {Time}. Next run: {NextRun}",
            DateTime.UtcNow,
            timer.ScheduleStatus?.Next);

        // TODO: Call application services to process pending reviews,
        // send digest emails, or run aggregations.
        await Task.Delay(100); // placeholder
    }
}
