using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces;
using ProductService.Domain.Events;

namespace ProductService.Infrastructure.Services;

public sealed class ProductCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<ProductCreatedEventHandler> logger) : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Product created - {Sku}", notification.Product.Sku);
        await publisher.PublishAsync("product.events", "product.created",
            new { notification.Product.ProductId, notification.Product.Sku, notification.Product.Name }, ct);
    }
}

public sealed class ProductUpdatedEventHandler(
    IMessagePublisher publisher,
    ILogger<ProductUpdatedEventHandler> logger) : INotificationHandler<ProductUpdatedEvent>
{
    public async Task Handle(ProductUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Product updated - {Sku}", notification.Product.Sku);
        await publisher.PublishAsync("product.events", "product.updated",
            new { notification.Product.ProductId, notification.Product.Sku, notification.Product.Name }, ct);
    }
}

public sealed class ProductDeactivatedEventHandler(
    IMessagePublisher publisher,
    ILogger<ProductDeactivatedEventHandler> logger) : INotificationHandler<ProductDeactivatedEvent>
{
    public async Task Handle(ProductDeactivatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Product deactivated - {Sku}", notification.Sku);
        await publisher.PublishAsync("product.events", "product.deactivated",
            new { notification.ProductId, notification.Sku }, ct);
    }
}
