using MediatR;
using Microsoft.Extensions.Logging;
using BatchAndEnvelopeService.Domain.Events;
using BatchAndEnvelopeService.Application.Interfaces;

namespace BatchAndEnvelopeService.Application.EventHandlers;

public class EnvelopeCreatedEventHandler : INotificationHandler<EnvelopeCreatedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<EnvelopeCreatedEventHandler> _logger;

    public EnvelopeCreatedEventHandler(IMessagePublisher publisher, ILogger<EnvelopeCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(EnvelopeCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DomainEvent] EnvelopeCreated: EnvelopeId={EnvelopeId}", notification.EnvelopeId);
        await _publisher.PublishAsync("envelope.exchange", "envelope.created", new
        {
            notification.EnvelopeId,
            notification.EnvelopeType,
            notification.CreatedBy,
            notification.LocationId,
            notification.OccurredOn
        });
    }
}

public class EnvelopeConfirmedEventHandler : INotificationHandler<EnvelopeConfirmedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<EnvelopeConfirmedEventHandler> _logger;

    public EnvelopeConfirmedEventHandler(IMessagePublisher publisher, ILogger<EnvelopeConfirmedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(EnvelopeConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DomainEvent] EnvelopeConfirmed: EnvelopeId={EnvelopeId}", notification.EnvelopeId);
        await _publisher.PublishAsync("envelope.exchange", "envelope.confirmed", new
        {
            notification.EnvelopeId,
            notification.ConfirmedBy,
            notification.OccurredOn
        });
    }
}

public class EnvelopeCancelledEventHandler : INotificationHandler<EnvelopeCancelledDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<EnvelopeCancelledEventHandler> _logger;

    public EnvelopeCancelledEventHandler(IMessagePublisher publisher, ILogger<EnvelopeCancelledEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(EnvelopeCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DomainEvent] EnvelopeCancelled: EnvelopeId={EnvelopeId}", notification.EnvelopeId);
        await _publisher.PublishAsync("envelope.exchange", "envelope.cancelled", new
        {
            notification.EnvelopeId,
            notification.CancelledBy,
            notification.OccurredOn
        });
    }
}
