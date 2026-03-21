using MediatR;
using Microsoft.Extensions.Logging;
using WarehouseStructure.Application.Interfaces;
using WarehouseStructure.Domain.Events;

namespace WarehouseStructure.Infrastructure.Services;

public class WarehouseCreatedEventHandler : INotificationHandler<WarehouseCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<WarehouseCreatedEventHandler> _logger;

    public WarehouseCreatedEventHandler(IMessagePublisher publisher, ILogger<WarehouseCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(WarehouseCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Warehouse created: {Code} - {Name}", notification.Code, notification.Name);
        await _publisher.PublishAsync("warehouse-events", "warehouse.created", notification, cancellationToken);
    }
}

public class WarehouseUpdatedEventHandler : INotificationHandler<WarehouseUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<WarehouseUpdatedEventHandler> _logger;

    public WarehouseUpdatedEventHandler(IMessagePublisher publisher, ILogger<WarehouseUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(WarehouseUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Warehouse updated: {Code}", notification.Code);
        await _publisher.PublishAsync("warehouse-events", "warehouse.updated", notification, cancellationToken);
    }
}

public class WarehouseDeletedEventHandler : INotificationHandler<WarehouseDeletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<WarehouseDeletedEventHandler> _logger;

    public WarehouseDeletedEventHandler(IMessagePublisher publisher, ILogger<WarehouseDeletedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(WarehouseDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Warehouse deleted: {Code}", notification.Code);
        await _publisher.PublishAsync("warehouse-events", "warehouse.deleted", notification, cancellationToken);
    }
}

public class ZoneCreatedEventHandler : INotificationHandler<ZoneCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ZoneCreatedEventHandler> _logger;

    public ZoneCreatedEventHandler(IMessagePublisher publisher, ILogger<ZoneCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ZoneCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Zone created: {Code} in warehouse {WarehouseId}", notification.Code, notification.WarehouseId);
        await _publisher.PublishAsync("warehouse-events", "zone.created", notification, cancellationToken);
    }
}
