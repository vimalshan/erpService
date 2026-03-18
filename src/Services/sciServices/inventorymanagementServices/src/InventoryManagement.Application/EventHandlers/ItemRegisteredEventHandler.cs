using InventoryManagement.Domain.Events;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.EventHandlers;

public sealed class ItemRegisteredEventHandler : INotificationHandler<ItemRegisteredEvent>
{
    private readonly ILogger<ItemRegisteredEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public ItemRegisteredEventHandler(ILogger<ItemRegisteredEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(ItemRegisteredEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: Item {ItemId} oracle code '{OracleCode}' registered at {OccurredAt}",
            notification.ItemId, notification.OracleCode, notification.OccurredAt);

        await _publisher.PublishAsync("inventory.item.registered.event", new
        {
            notification.ItemId,
            notification.OracleCode,
            notification.ItemName,
            notification.OccurredAt
        }, ct);
    }
}
