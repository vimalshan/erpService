using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ScheduleService.Domain.Events;

namespace ScheduleService.Infrastructure.Messaging;

/// <summary>
/// MediatR notification handler that bridges AuditScheduledEvent domain events to MassTransit/RabbitMQ.
/// </summary>
public class AuditScheduledEventHandler : INotificationHandler<AuditScheduledEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AuditScheduledEventHandler> _logger;

    public AuditScheduledEventHandler(IPublishEndpoint publishEndpoint, ILogger<AuditScheduledEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(AuditScheduledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing AuditScheduledEvent for AuditSiteAuditId={Id}", notification.AuditSiteAuditId);
        await _publishEndpoint.Publish(notification, cancellationToken);
    }
}

/// <summary>
/// MediatR notification handler that bridges AuditCompletedEvent domain events to MassTransit/RabbitMQ.
/// </summary>
public class AuditCompletedEventHandler : INotificationHandler<AuditCompletedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AuditCompletedEventHandler> _logger;

    public AuditCompletedEventHandler(IPublishEndpoint publishEndpoint, ILogger<AuditCompletedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(AuditCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing AuditCompletedEvent for AuditSiteAuditId={Id}", notification.AuditSiteAuditId);
        await _publishEndpoint.Publish(notification, cancellationToken);
    }
}
