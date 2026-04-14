using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using WMTransactional.Domain.Events;

namespace WMTransactional.Infrastructure.Messaging.EventHandlers;

public class ShipmentCreatedEventHandler : INotificationHandler<ShipmentCreatedEvent>
{
    private readonly ILogger<ShipmentCreatedEventHandler> _logger;

    public ShipmentCreatedEventHandler(ILogger<ShipmentCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ShipmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shipment {ShipmentNumber} created for SO {SoId}", notification.ShipmentNumber, notification.SoId);
        return Task.CompletedTask;
    }
}

public class ShipmentShippedEventHandler : INotificationHandler<ShipmentShippedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ShipmentShippedEventHandler> _logger;

    public ShipmentShippedEventHandler(IPublishEndpoint publishEndpoint, ILogger<ShipmentShippedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ShipmentShippedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing ShipmentShippedMessage for Shipment {ShipmentNumber}", notification.ShipmentNumber);
        await _publishEndpoint.Publish(new ShipmentShippedMessage
        {
            ShipmentNumber = notification.ShipmentNumber,
            SoId = notification.SoId,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class ShipmentCancelledEventHandler : INotificationHandler<ShipmentCancelledEvent>
{
    private readonly ILogger<ShipmentCancelledEventHandler> _logger;

    public ShipmentCancelledEventHandler(ILogger<ShipmentCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ShipmentCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shipment {ShipmentNumber} cancelled for SO {SoId}", notification.ShipmentNumber, notification.SoId);
        return Task.CompletedTask;
    }
}
