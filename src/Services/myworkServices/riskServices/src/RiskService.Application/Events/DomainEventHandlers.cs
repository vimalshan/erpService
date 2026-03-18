using MediatR;
using RiskService.Domain.Events;
using RiskService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace RiskService.Application.Events;

public class RiskCreatedEventHandler(IMessagePublisher publisher, ILogger<RiskCreatedEventHandler> logger)
    : INotificationHandler<RiskCreatedEvent>
{
    public async Task Handle(RiskCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Risk {RiskId} created with title '{Title}'", notification.RiskId, notification.Title);
        await publisher.PublishAsync("risk.events", "risk.created",
            new { notification.RiskId, notification.Title, notification.OccurredOn }, cancellationToken);
    }
}

public class RiskApprovedEventHandler(IMessagePublisher publisher, ILogger<RiskApprovedEventHandler> logger)
    : INotificationHandler<RiskApprovedEvent>
{
    public async Task Handle(RiskApprovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Risk {RiskId} approved by {ApprovedBy}", notification.RiskId, notification.ApprovedBy);
        await publisher.PublishAsync("risk.events", "risk.approved",
            new { notification.RiskId, notification.ApprovedBy, notification.OccurredOn }, cancellationToken);
    }
}

public class RiskCancelledEventHandler(IMessagePublisher publisher, ILogger<RiskCancelledEventHandler> logger)
    : INotificationHandler<RiskCancelledEvent>
{
    public async Task Handle(RiskCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Risk {RiskId} cancelled. Reason: {Reason}", notification.RiskId, notification.Reason);
        await publisher.PublishAsync("risk.events", "risk.cancelled",
            new { notification.RiskId, notification.Reason, notification.OccurredOn }, cancellationToken);
    }
}

public class RiskMitigationAddedEventHandler(IMessagePublisher publisher, ILogger<RiskMitigationAddedEventHandler> logger)
    : INotificationHandler<RiskMitigationAddedEvent>
{
    public async Task Handle(RiskMitigationAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Mitigation {MitigationId} added to Risk {RiskId}", notification.MitigationId, notification.RiskId);
        await publisher.PublishAsync("risk.events", "risk.mitigation.added",
            new { notification.RiskId, notification.MitigationId, notification.OccurredOn }, cancellationToken);
    }
}
