using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OtherService.Application.CQRS.Queries.GetAllLogDdCatDevDetails;

namespace OtherService.Functions.Functions;

/// <summary>
/// Timer-triggered background task that runs every hour.
/// Example: query all log entries and produce metrics or archive stale records.
/// </summary>
public sealed class LogProcessingFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<LogProcessingFunction> _logger;

    public LogProcessingFunction(
        IMediator mediator,
        ILogger<LogProcessingFunction> logger)
    {
        _mediator = mediator;
        _logger   = logger;
    }

    [Function(nameof(LogProcessingFunction))]
    public async Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "LogProcessingFunction triggered at {Time}", DateTimeOffset.UtcNow);

        var entries = await _mediator.Send(new GetAllLogDdCatDevDetailsQuery(), ct);
        var count   = entries.Count();

        _logger.LogInformation(
            "LogProcessingFunction: processed {Count} log entries.", count);
    }
}
