using MediatR;
using Microsoft.Extensions.Logging;
using PurchaseOrderService.Application.Interfaces;
using PurchaseOrderService.Domain.Events;

namespace PurchaseOrderService.Infrastructure.EventHandlers;

public class PurchaseOrderCreatedEventHandler : INotificationHandler<PurchaseOrderCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<PurchaseOrderCreatedEventHandler> _logger;

    public PurchaseOrderCreatedEventHandler(IMessagePublisher publisher, ILogger<PurchaseOrderCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: PurchaseOrder {PoNumber} created", notification.PoNumber);
        await _publisher.PublishAsync("erp.exchange", "purchaseorder.created", notification, cancellationToken);
    }
}

public class PurchaseOrderConfirmedEventHandler : INotificationHandler<PurchaseOrderConfirmedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<PurchaseOrderConfirmedEventHandler> _logger;

    public PurchaseOrderConfirmedEventHandler(IMessagePublisher publisher, ILogger<PurchaseOrderConfirmedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: PurchaseOrder {PoNumber} confirmed", notification.PoNumber);
        await _publisher.PublishAsync("erp.exchange", "purchaseorder.confirmed", notification, cancellationToken);
    }
}

public class PurchaseOrderCompletedEventHandler : INotificationHandler<PurchaseOrderCompletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<PurchaseOrderCompletedEventHandler> _logger;

    public PurchaseOrderCompletedEventHandler(IMessagePublisher publisher, ILogger<PurchaseOrderCompletedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: PurchaseOrder {PoNumber} completed", notification.PoNumber);
        await _publisher.PublishAsync("erp.exchange", "purchaseorder.completed", notification, cancellationToken);
    }
}

public class PurchaseOrderCancelledEventHandler : INotificationHandler<PurchaseOrderCancelledEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<PurchaseOrderCancelledEventHandler> _logger;

    public PurchaseOrderCancelledEventHandler(IMessagePublisher publisher, ILogger<PurchaseOrderCancelledEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: PurchaseOrder {PoNumber} cancelled", notification.PoNumber);
        await _publisher.PublishAsync("erp.exchange", "purchaseorder.cancelled", notification, cancellationToken);
    }
}
