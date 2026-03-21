using MediatR;
using Microsoft.Extensions.Logging;
using UnitService.Application.Interfaces;
using UnitService.Domain.Events;

namespace UnitService.Application.EventHandlers;

public class AccessGrantedEventHandler : INotificationHandler<AccessGrantedEvent>
{
    private readonly ILogger<AccessGrantedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public AccessGrantedEventHandler(ILogger<AccessGrantedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(AccessGrantedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Access granted for Employee {EmpId} to Unit {UnitCode}",
            notification.EmployeeSysId, notification.UnitCode);

        await _messagePublisher.PublishAsync(notification, "unit-events", "access.granted", cancellationToken);
    }
}
