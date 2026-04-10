using MediatR;
using Microsoft.Extensions.Logging;
using ApprovalGroup.Domain.Events;
using ApprovalGroup.Domain.Interfaces;

namespace ApprovalGroup.Application.ApprovalGroups.EventHandlers;

public class ApprovalGroupCreatedEventHandler : INotificationHandler<ApprovalGroupCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ApprovalGroupCreatedEventHandler> _logger;

    public ApprovalGroupCreatedEventHandler(IMessagePublisher publisher, ILogger<ApprovalGroupCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ApprovalGroupCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: ApprovalGroup created – Id={GroupId}, Name={GroupName}",
            notification.GroupId, notification.GroupName);
        await _publisher.PublishAsync(new { notification.GroupId, notification.GroupName }, "approval_group.created", ct);
    }
}

public class ApprovalGroupUpdatedEventHandler : INotificationHandler<ApprovalGroupUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ApprovalGroupUpdatedEventHandler> _logger;

    public ApprovalGroupUpdatedEventHandler(IMessagePublisher publisher, ILogger<ApprovalGroupUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ApprovalGroupUpdatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: ApprovalGroup updated – Id={GroupId}, Name={GroupName}",
            notification.GroupId, notification.GroupName);
        await _publisher.PublishAsync(new { notification.GroupId, notification.GroupName }, "approval_group.updated", ct);
    }
}

public class UserMappedToGroupEventHandler : INotificationHandler<UserMappedToGroupEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<UserMappedToGroupEventHandler> _logger;

    public UserMappedToGroupEventHandler(IMessagePublisher publisher, ILogger<UserMappedToGroupEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(UserMappedToGroupEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: User {UserId} mapped to group {GroupId}",
            notification.UserId, notification.GroupId);
        await _publisher.PublishAsync(new { notification.GroupId, notification.UserId }, "approval_group.user_mapped", ct);
    }
}
