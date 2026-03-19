using MediatR;
using Microsoft.Extensions.Logging;
using BatchAndEnvelopeService.Domain.Events;
using BatchAndEnvelopeService.Application.Interfaces;

namespace BatchAndEnvelopeService.Application.EventHandlers;

public class BatchCreatedEventHandler : INotificationHandler<BatchCreatedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<BatchCreatedEventHandler> _logger;

    public BatchCreatedEventHandler(IMessagePublisher publisher, ILogger<BatchCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(BatchCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DomainEvent] BatchCreated: BatchId={BatchId}", notification.BatchId);
        await _publisher.PublishAsync("batch.exchange", "batch.created", new
        {
            notification.BatchId,
            notification.CreatedBy,
            notification.LocationId,
            notification.OccurredOn
        });
    }
}

public class BatchConfirmedEventHandler : INotificationHandler<BatchConfirmedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<BatchConfirmedEventHandler> _logger;

    public BatchConfirmedEventHandler(IMessagePublisher publisher, ILogger<BatchConfirmedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(BatchConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DomainEvent] BatchConfirmed: BatchId={BatchId}", notification.BatchId);
        await _publisher.PublishAsync("batch.exchange", "batch.confirmed", new
        {
            notification.BatchId,
            notification.ConfirmedBy,
            notification.OccurredOn
        });
    }
}

public class BatchCancelledEventHandler : INotificationHandler<BatchCancelledDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<BatchCancelledEventHandler> _logger;

    public BatchCancelledEventHandler(IMessagePublisher publisher, ILogger<BatchCancelledEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(BatchCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DomainEvent] BatchCancelled: BatchId={BatchId}", notification.BatchId);
        await _publisher.PublishAsync("batch.exchange", "batch.cancelled", new
        {
            notification.BatchId,
            notification.CancelledBy,
            notification.OccurredOn
        });
    }
}
