using CanteenUnit.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CanteenUnit.Application.EventHandlers;

public class CanteenUnitCreatedEventHandler : INotificationHandler<CanteenUnitCreatedEvent>
{
    private readonly ILogger<CanteenUnitCreatedEventHandler> _logger;
    public CanteenUnitCreatedEventHandler(ILogger<CanteenUnitCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(CanteenUnitCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CanteenUnit created — Code={Code}, Name={Name} at {Time}",
            notification.CompanyCode, notification.UnitName, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class CanteenUnitUpdatedEventHandler : INotificationHandler<CanteenUnitUpdatedEvent>
{
    private readonly ILogger<CanteenUnitUpdatedEventHandler> _logger;
    public CanteenUnitUpdatedEventHandler(ILogger<CanteenUnitUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(CanteenUnitUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CanteenUnit updated — Code={Code}, {Old} → {New}",
            notification.CompanyCode, notification.OldUnitName, notification.NewUnitName);
        return Task.CompletedTask;
    }
}

public class CanteenAccessGrantedEventHandler : INotificationHandler<CanteenAccessGrantedEvent>
{
    private readonly ILogger<CanteenAccessGrantedEventHandler> _logger;
    public CanteenAccessGrantedEventHandler(ILogger<CanteenAccessGrantedEventHandler> logger) => _logger = logger;

    public Task Handle(CanteenAccessGrantedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Access granted — Unit={Unit}, User={User}, Access={Acc}",
            notification.CompanyCode, notification.UserId, notification.AccessNumber);
        return Task.CompletedTask;
    }
}
