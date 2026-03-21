using MediatR;
using Microsoft.Extensions.Logging;
using ReceivingService.Domain.Events;

namespace ReceivingService.Application.EventHandlers;

/// <summary>
/// Handles ReceivingCreatedEvent – publishes an integration event to RabbitMQ
/// and logs the creation.
/// </summary>
public sealed class ReceivingCreatedEventHandler
    : INotificationHandler<ReceivingCreatedEvent>
{
    private readonly ILogger<ReceivingCreatedEventHandler> _logger;

    public ReceivingCreatedEventHandler(ILogger<ReceivingCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ReceivingCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Receiving created. ReceivingNumber={ReceivingNumber}, PoId={PoId}",
            notification.Receiving.ReceivingNumber,
            notification.Receiving.PoId);

        // TODO: Publish integration event to messaging broker.
        return Task.CompletedTask;
    }
}

/// <summary>Handles ReceivingClosedEvent.</summary>
public sealed class ReceivingClosedEventHandler
    : INotificationHandler<ReceivingClosedEvent>
{
    private readonly ILogger<ReceivingClosedEventHandler> _logger;

    public ReceivingClosedEventHandler(ILogger<ReceivingClosedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ReceivingClosedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Receiving closed. ReceivingId={ReceivingId}",
            notification.Receiving.Id);
        return Task.CompletedTask;
    }
}

/// <summary>Handles ReceivingCancelledEvent.</summary>
public sealed class ReceivingCancelledEventHandler
    : INotificationHandler<ReceivingCancelledEvent>
{
    private readonly ILogger<ReceivingCancelledEventHandler> _logger;

    public ReceivingCancelledEventHandler(ILogger<ReceivingCancelledEventHandler> logger)
        => _logger = logger;

    public Task Handle(ReceivingCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Receiving cancelled. ReceivingId={ReceivingId}",
            notification.Receiving.Id);
        return Task.CompletedTask;
    }
}
