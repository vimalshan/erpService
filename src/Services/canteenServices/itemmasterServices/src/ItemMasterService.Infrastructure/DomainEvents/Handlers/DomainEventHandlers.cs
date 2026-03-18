using MediatR;
using Microsoft.Extensions.Logging;
using ItemMasterService.Domain.Events;
using ItemMasterService.Domain.Interfaces;

namespace ItemMasterService.Infrastructure.DomainEvents.Handlers;

public class CanteenItemCreatedEventHandler : INotificationHandler<CanteenItemCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CanteenItemCreatedEventHandler> _logger;

    public CanteenItemCreatedEventHandler(IMessagePublisher publisher, ILogger<CanteenItemCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CanteenItemCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] CanteenItemCreated: ItemCode={ItemCode}", notification.ItemCode);
        await _publisher.PublishAsync(notification, "canteen.item.created", ct);
    }
}

public class CanteenItemPriceUpdatedEventHandler : INotificationHandler<CanteenItemPriceUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CanteenItemPriceUpdatedEventHandler> _logger;

    public CanteenItemPriceUpdatedEventHandler(IMessagePublisher publisher, ILogger<CanteenItemPriceUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CanteenItemPriceUpdatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] PriceUpdated: ItemCode={ItemCode}", notification.ItemCode);
        await _publisher.PublishAsync(notification, "canteen.item.price.updated", ct);
    }
}
