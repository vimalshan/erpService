using MediatR;
using Microsoft.Extensions.Logging;
using UnitService.Application.Interfaces;
using UnitService.Domain.Events;

namespace UnitService.Application.EventHandlers;

public class EquipmentStatusChangedEventHandler : INotificationHandler<EquipmentStatusChangedEvent>
{
    private readonly ILogger<EquipmentStatusChangedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public EquipmentStatusChangedEventHandler(ILogger<EquipmentStatusChangedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(EquipmentStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Equipment {EquipmentId} status changed to {Status}",
            notification.EquipmentId, notification.StatusCode);

        await _messagePublisher.PublishAsync(notification, "unit-events", "equipment.status.changed", cancellationToken);
    }
}
