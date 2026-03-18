using InventoryManagement.Domain.Events;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.EventHandlers;

public sealed class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: Product {ProductId} '{ProductName}' created at {OccurredAt}",
            notification.ProductId, notification.ProductName, notification.OccurredAt);

        await _publisher.PublishAsync("inventory.product.created.event", new
        {
            notification.ProductId,
            notification.ProductName,
            notification.OccurredAt
        }, ct);
    }
}
