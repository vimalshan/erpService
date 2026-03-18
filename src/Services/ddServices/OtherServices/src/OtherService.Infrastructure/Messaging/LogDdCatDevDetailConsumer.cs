using MassTransit;
using Microsoft.Extensions.Logging;
using OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;
using MediatR;

namespace OtherService.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message contract for incoming log entries.
/// </summary>
public sealed record LogDdCatDevDetailMessage
{
    public decimal? ReqNum { get; init; }
    public decimal? QtnNum { get; init; }
    public decimal? AnsSrl { get; init; }
    public string AppId { get; init; } = default!;
    public decimal AppNum { get; init; }
    public DateTime? EntDat { get; init; }
    public string? Desc { get; init; }
    public string? Need { get; init; }

    // Public parameterless constructor for MassTransit deserialization
    public LogDdCatDevDetailMessage() { }

    // Positional constructor for convenience
    public LogDdCatDevDetailMessage(
        decimal? reqNum,
        decimal? qtnNum,
        decimal? ansSrl,
        string appId,
        decimal appNum,
        DateTime? entDat,
        string? desc,
        string? need)
    {
        ReqNum = reqNum;
        QtnNum = qtnNum;
        AnsSrl = ansSrl;
        AppId = appId;
        AppNum = appNum;
        EntDat = entDat;
        Desc = desc;
        Need = need;
    }
}

/// <summary>
/// MassTransit consumer for <see cref="LogDdCatDevDetailMessage"/> events from RabbitMQ.
/// Implements Circuit Breaker tolerance — bad messages are observed but not crashed upon.
/// </summary>
public sealed class LogDdCatDevDetailConsumer : IConsumer<LogDdCatDevDetailMessage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<LogDdCatDevDetailConsumer> _logger;

    public LogDdCatDevDetailConsumer(
        IMediator mediator,
        ILogger<LogDdCatDevDetailConsumer> logger)
    {
        _mediator = mediator;
        _logger   = logger;
    }

    public async Task Consume(ConsumeContext<LogDdCatDevDetailMessage> context)
    {
        _logger.LogInformation(
            "Received LogDdCatDevDetailMessage for AppId={AppId} AppNum={AppNum}",
            context.Message.AppId,
            context.Message.AppNum);

        var command = new CreateLogDdCatDevDetailCommand(
            context.Message.ReqNum,
            context.Message.QtnNum,
            context.Message.AnsSrl,
            context.Message.AppId,
            context.Message.AppNum,
            context.Message.EntDat,
            context.Message.Desc,
            context.Message.Need);

        await _mediator.Send(command, context.CancellationToken);
    }
}
