using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using WMTransactional.Domain.Events;

namespace WMTransactional.Infrastructure.Messaging.EventHandlers;

public class PurchaseOrderCreatedEventHandler : INotificationHandler<PurchaseOrderCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PurchaseOrderCreatedEventHandler> _logger;

    public PurchaseOrderCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<PurchaseOrderCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing PurchaseOrderCreatedMessage for PO {PoNumber}", notification.PoNumber);
        await _publishEndpoint.Publish(new PurchaseOrderCreatedMessage
        {
            PoNumber = notification.PoNumber,
            SupplierId = notification.SupplierId,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class PurchaseOrderConfirmedEventHandler : INotificationHandler<PurchaseOrderConfirmedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PurchaseOrderConfirmedEventHandler> _logger;

    public PurchaseOrderConfirmedEventHandler(IPublishEndpoint publishEndpoint, ILogger<PurchaseOrderConfirmedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing PurchaseOrderStatusChangedMessage for PO {PoNumber} -> CONFIRMED", notification.PoNumber);
        await _publishEndpoint.Publish(new PurchaseOrderStatusChangedMessage
        {
            PoNumber = notification.PoNumber,
            SupplierId = notification.SupplierId,
            NewStatus = "CONFIRMED",
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class PurchaseOrderCompletedEventHandler : INotificationHandler<PurchaseOrderCompletedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PurchaseOrderCompletedEventHandler> _logger;

    public PurchaseOrderCompletedEventHandler(IPublishEndpoint publishEndpoint, ILogger<PurchaseOrderCompletedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing PurchaseOrderStatusChangedMessage for PO {PoNumber} -> COMPLETED", notification.PoNumber);
        await _publishEndpoint.Publish(new PurchaseOrderStatusChangedMessage
        {
            PoNumber = notification.PoNumber,
            SupplierId = notification.SupplierId,
            NewStatus = "COMPLETED",
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class PurchaseOrderCancelledEventHandler : INotificationHandler<PurchaseOrderCancelledEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PurchaseOrderCancelledEventHandler> _logger;

    public PurchaseOrderCancelledEventHandler(IPublishEndpoint publishEndpoint, ILogger<PurchaseOrderCancelledEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(PurchaseOrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing PurchaseOrderStatusChangedMessage for PO {PoNumber} -> CANCELLED", notification.PoNumber);
        await _publishEndpoint.Publish(new PurchaseOrderStatusChangedMessage
        {
            PoNumber = notification.PoNumber,
            SupplierId = notification.SupplierId,
            NewStatus = "CANCELLED",
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}
