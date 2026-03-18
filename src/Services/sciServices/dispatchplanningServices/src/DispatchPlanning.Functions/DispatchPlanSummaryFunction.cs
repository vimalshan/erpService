using DispatchPlanning.Infrastructure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DispatchPlanning.Functions;

public class DispatchPlanSummaryFunction
{
    private readonly ILogger<DispatchPlanSummaryFunction> _logger;
    private readonly IMessagePublisher _publisher;

    public DispatchPlanSummaryFunction(ILogger<DispatchPlanSummaryFunction> logger,
        IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    /// <summary>
    /// Runs every day at midnight: publishes a scheduled dispatch plan summary event to RabbitMQ.
    /// </summary>
    [Function(nameof(DispatchPlanDailySummary))]
    public async Task DispatchPlanDailySummary(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        _logger.LogInformation("[AzFunc] DispatchPlanDailySummary triggered at: {Time}", DateTime.UtcNow);

        var summaryEvent = new
        {
            Type = "DailySummary",
            GeneratedAt = DateTime.UtcNow,
            Message = "Dispatch plan daily summary run"
        };

        await _publisher.PublishAsync(
            "dispatch.planning.events",
            "dispatch.plan.daily.summary",
            summaryEvent,
            ct);

        _logger.LogInformation("[AzFunc] Daily summary event published successfully.");
    }
}
