using MediatR;
using Microsoft.Extensions.Logging;
using UnitService.Application.Interfaces;
using UnitService.Domain.Events;

namespace UnitService.Application.EventHandlers;

public class EquipmentRegisteredEventHandler : INotificationHandler<EquipmentRegisteredEvent>
{
    private readonly ILogger<EquipmentRegisteredEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public EquipmentRegisteredEventHandler(ILogger<EquipmentRegisteredEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(EquipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Equipment {EquipmentId} registered - {Name}",
            notification.EquipmentId, notification.EquipmentName);

        await _messagePublisher.PublishAsync(notification, "unit-events", "equipment.registered", cancellationToken);
    }
}
