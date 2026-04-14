using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using WMTransactional.Domain.Events;

namespace WMTransactional.Infrastructure.Messaging.EventHandlers;

public class ReceivingCreatedEventHandler : INotificationHandler<ReceivingCreatedEvent>
{
    private readonly ILogger<ReceivingCreatedEventHandler> _logger;

    public ReceivingCreatedEventHandler(ILogger<ReceivingCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ReceivingCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receiving {ReceivingNumber} created for PO {PoId}", notification.ReceivingNumber, notification.PoId);
        return Task.CompletedTask;
    }
}

public class ReceivingClosedEventHandler : INotificationHandler<ReceivingClosedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ReceivingClosedEventHandler> _logger;

    public ReceivingClosedEventHandler(IPublishEndpoint publishEndpoint, ILogger<ReceivingClosedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ReceivingClosedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing ReceivingCompletedMessage for Receiving {ReceivingNumber}", notification.ReceivingNumber);
        await _publishEndpoint.Publish(new ReceivingCompletedMessage
        {
            ReceivingNumber = notification.ReceivingNumber,
            PoId = notification.PoId,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class ReceivingCancelledEventHandler : INotificationHandler<ReceivingCancelledEvent>
{
    private readonly ILogger<ReceivingCancelledEventHandler> _logger;

    public ReceivingCancelledEventHandler(ILogger<ReceivingCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ReceivingCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receiving {ReceivingNumber} cancelled for PO {PoId}", notification.ReceivingNumber, notification.PoId);
        return Task.CompletedTask;
    }
}
