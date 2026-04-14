using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using WMTransactional.Domain.Events;

namespace WMTransactional.Infrastructure.Messaging.EventHandlers;

public class SalesOrderCreatedEventHandler : INotificationHandler<SalesOrderCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SalesOrderCreatedEventHandler> _logger;

    public SalesOrderCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<SalesOrderCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(SalesOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing SalesOrderCreatedMessage for SO {SoNumber}", notification.SoNumber);
        await _publishEndpoint.Publish(new SalesOrderCreatedMessage
        {
            SoNumber = notification.SoNumber,
            CustomerId = notification.CustomerId,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class SalesOrderConfirmedEventHandler : INotificationHandler<SalesOrderConfirmedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SalesOrderConfirmedEventHandler> _logger;

    public SalesOrderConfirmedEventHandler(IPublishEndpoint publishEndpoint, ILogger<SalesOrderConfirmedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(SalesOrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing SalesOrderStatusChangedMessage for SO {SoNumber} -> CONFIRMED", notification.SoNumber);
        await _publishEndpoint.Publish(new SalesOrderStatusChangedMessage
        {
            SoNumber = notification.SoNumber,
            CustomerId = notification.CustomerId,
            NewStatus = "CONFIRMED",
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class SalesOrderCompletedEventHandler : INotificationHandler<SalesOrderCompletedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SalesOrderCompletedEventHandler> _logger;

    public SalesOrderCompletedEventHandler(IPublishEndpoint publishEndpoint, ILogger<SalesOrderCompletedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(SalesOrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing SalesOrderStatusChangedMessage for SO {SoNumber} -> COMPLETED", notification.SoNumber);
        await _publishEndpoint.Publish(new SalesOrderStatusChangedMessage
        {
            SoNumber = notification.SoNumber,
            CustomerId = notification.CustomerId,
            NewStatus = "COMPLETED",
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class SalesOrderCancelledEventHandler : INotificationHandler<SalesOrderCancelledEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SalesOrderCancelledEventHandler> _logger;

    public SalesOrderCancelledEventHandler(IPublishEndpoint publishEndpoint, ILogger<SalesOrderCancelledEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(SalesOrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing SalesOrderStatusChangedMessage for SO {SoNumber} -> CANCELLED", notification.SoNumber);
        await _publishEndpoint.Publish(new SalesOrderStatusChangedMessage
        {
            SoNumber = notification.SoNumber,
            CustomerId = notification.CustomerId,
            NewStatus = "CANCELLED",
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}
