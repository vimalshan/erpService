using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Interfaces;
using OrderService.Domain.Events;

namespace OrderService.Infrastructure.EventHandlers;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order created: {OrderNumber}", notification.Order.OrderNumber);
        await _publisher.PublishAsync("orders", "order.created",
            new { notification.Order.OrderId, notification.Order.OrderNumber, notification.Order.CustomerId },
            cancellationToken);
    }
}

public class OrderStatusChangedEventHandler : INotificationHandler<OrderStatusChangedEvent>
{
    private readonly ILogger<OrderStatusChangedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public OrderStatusChangedEventHandler(ILogger<OrderStatusChangedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order {OrderId} status changed from {Previous} to {New}",
            notification.OrderId, notification.PreviousStatus, notification.NewStatus);
        await _publisher.PublishAsync("orders", "order.status.changed",
            new { notification.OrderId, Previous = notification.PreviousStatus.ToString(), New = notification.NewStatus.ToString() },
            cancellationToken);
    }
}
