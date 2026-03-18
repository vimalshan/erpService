using AuditService.Domain.Events;
using AuditService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditService.Infrastructure.Messaging.Consumers;

/// <summary>Publishes an external event when a new audit is created.</summary>
public sealed class AuditCreatedEventHandler : INotificationHandler<AuditCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AuditCreatedEventHandler> _logger;

    public AuditCreatedEventHandler(IMessagePublisher publisher, ILogger<AuditCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AuditCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: AuditCreated {AuditId} '{AuditName}'", notification.AuditId, notification.AuditName);

        await _publisher.PublishAsync(
            exchangeName: "audit.events",
            routingKey: "audit.created",
            message: new { notification.AuditId, notification.AuditName, OccurredOn = notification.OccurredOn },
            cancellationToken: cancellationToken);
    }
}

/// <summary>Handles observation status changes and notifies interested parties.</summary>
public sealed class ObservationStatusChangedEventHandler : INotificationHandler<ObservationStatusChangedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ObservationStatusChangedEventHandler> _logger;

    public ObservationStatusChangedEventHandler(IMessagePublisher publisher, ILogger<ObservationStatusChangedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ObservationStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event: ObservationStatusChanged {ObvId} {OldStatus}->{NewStatus}",
            notification.ObservationId, notification.OldStatus, notification.NewStatus);

        await _publisher.PublishAsync(
            exchangeName: "audit.events",
            routingKey: "observation.status.changed",
            message: new
            {
                notification.ObservationId,
                notification.AuditId,
                OldStatus = notification.OldStatus.ToString(),
                NewStatus = notification.NewStatus.ToString(),
                OccurredOn = notification.OccurredOn
            },
            cancellationToken: cancellationToken);
    }
}

/// <summary>Handles observation creation events.</summary>
public sealed class ObservationCreatedEventHandler : INotificationHandler<ObservationCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ObservationCreatedEventHandler> _logger;

    public ObservationCreatedEventHandler(IMessagePublisher publisher, ILogger<ObservationCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ObservationCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: ObservationCreated {ObvId} in Audit {AuditId}", notification.ObservationId, notification.AuditId);

        await _publisher.PublishAsync(
            exchangeName: "audit.events",
            routingKey: "observation.created",
            message: new { notification.ObservationId, notification.AuditId, notification.Title, OccurredOn = notification.OccurredOn },
            cancellationToken: cancellationToken);
    }
}
