using MediatR;
using Microsoft.Extensions.Logging;
using SalesOrderService.Domain.Events;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.EventHandlers;

public sealed class SalesOrderCreatedEventHandler(
    ILogger<SalesOrderCreatedEventHandler> logger,
    IEventBus eventBus)
    : INotificationHandler<SalesOrderCreatedDomainNotification>
{
    public async Task Handle(SalesOrderCreatedDomainNotification notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Sales order created: {SoNumber} for Customer {CustomerId}",
            notification.Event.SoNumber, notification.Event.CustomerId);

        // Publish integration event to the bus
        await eventBus.PublishAsync(new
        {
            notification.Event.SoNumber,
            notification.Event.CustomerId,
            notification.Event.OccurredOn,
            EventType = "SalesOrderCreated"
        }, cancellationToken);
    }
}

public sealed class SalesOrderConfirmedEventHandler(
    ILogger<SalesOrderConfirmedEventHandler> logger,
    IEventBus eventBus)
    : INotificationHandler<SalesOrderConfirmedDomainNotification>
{
    public async Task Handle(SalesOrderConfirmedDomainNotification notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Sales order confirmed: {SoNumber}", notification.Event.SoNumber);
        await eventBus.PublishAsync(new
        {
            notification.Event.SoNumber,
            notification.Event.CustomerId,
            notification.Event.OccurredOn,
            EventType = "SalesOrderConfirmed"
        }, cancellationToken);
    }
}

// ── Domain Notification Wrappers (MediatR INotification) ────────────────────
public sealed record SalesOrderCreatedDomainNotification(SalesOrderCreatedEvent Event) : INotification;
public sealed record SalesOrderConfirmedDomainNotification(SalesOrderConfirmedEvent Event) : INotification;
public sealed record SalesOrderCompletedDomainNotification(SalesOrderCompletedEvent Event) : INotification;
public sealed record SalesOrderCancelledDomainNotification(SalesOrderCancelledEvent Event) : INotification;
