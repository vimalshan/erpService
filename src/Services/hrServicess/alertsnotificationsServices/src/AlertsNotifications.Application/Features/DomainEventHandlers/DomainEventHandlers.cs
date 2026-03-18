using AlertsNotifications.Application.Interfaces;
using AlertsNotifications.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlertsNotifications.Application.Features.DomainEventHandlers;

public class AlertCreatedEventHandler : INotificationHandler<AlertCreatedEvent>
{
    private readonly ILogger<AlertCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public AlertCreatedEventHandler(ILogger<AlertCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(AlertCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Alert created - ID: {AlertId}, Name: {AlertName}",
            notification.AlertId, notification.AlertName);

        await _messagePublisher.PublishAsync(
            "alerts-notifications-exchange",
            "alert.notification.created",
            new { notification.AlertId, notification.AlertName, notification.OccurredOn },
            cancellationToken);
    }
}

public class AlertGroupCreatedEventHandler : INotificationHandler<AlertGroupCreatedEvent>
{
    private readonly ILogger<AlertGroupCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public AlertGroupCreatedEventHandler(ILogger<AlertGroupCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(AlertGroupCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Alert Group created - ID: {GroupId}, Name: {GroupName}",
            notification.AlertGroupId, notification.AlertGroupName);

        await _messagePublisher.PublishAsync(
            "alerts-notifications-exchange",
            "alert.notification.group-created",
            new { notification.AlertGroupId, notification.AlertGroupName, notification.OccurredOn },
            cancellationToken);
    }
}

public class CircularApprovedEventHandler : INotificationHandler<CircularApprovedEvent>
{
    private readonly ILogger<CircularApprovedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public CircularApprovedEventHandler(ILogger<CircularApprovedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(CircularApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Circular approved - ID: {CircularId}, Subject: {Subject}, ApprovedBy: {ApprovedBy}",
            notification.CircularId, notification.CircularSubject, notification.ApprovedBy);

        await _messagePublisher.PublishAsync(
            "alerts-notifications-exchange",
            "circular.approval.approved",
            new { notification.CircularId, notification.CircularSubject, notification.ApprovedBy, notification.OccurredOn },
            cancellationToken);
    }
}

public class CircularStatusChangedEventHandler : INotificationHandler<CircularStatusChangedEvent>
{
    private readonly ILogger<CircularStatusChangedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public CircularStatusChangedEventHandler(ILogger<CircularStatusChangedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(CircularStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Circular status changed - ID: {CircularId}, OldStatus: {Old}, NewStatus: {New}",
            notification.CircularId, notification.OldStatus, notification.NewStatus);

        await _messagePublisher.PublishAsync(
            "alerts-notifications-exchange",
            "circular.approval.status-changed",
            new { notification.CircularId, notification.OldStatus, notification.NewStatus, notification.OccurredOn },
            cancellationToken);
    }
}
