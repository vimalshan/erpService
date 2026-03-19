using MediatR;
using Microsoft.Extensions.Logging;
using ApprovalGroup.Domain.Events;

namespace ApprovalGroup.Application.ApprovalGroups.EventHandlers;

public class ApprovalGroupCreatedEventHandler : INotificationHandler<ApprovalGroupCreatedEvent>
{
    private readonly ILogger<ApprovalGroupCreatedEventHandler> _logger;

    public ApprovalGroupCreatedEventHandler(ILogger<ApprovalGroupCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(ApprovalGroupCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: ApprovalGroup created – Id={GroupId}, Name={GroupName}",
            notification.GroupId, notification.GroupName);
        return Task.CompletedTask;
    }
}

public class ApprovalGroupUpdatedEventHandler : INotificationHandler<ApprovalGroupUpdatedEvent>
{
    private readonly ILogger<ApprovalGroupUpdatedEventHandler> _logger;

    public ApprovalGroupUpdatedEventHandler(ILogger<ApprovalGroupUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(ApprovalGroupUpdatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: ApprovalGroup updated – Id={GroupId}, Name={GroupName}",
            notification.GroupId, notification.GroupName);
        return Task.CompletedTask;
    }
}

public class UserMappedToGroupEventHandler : INotificationHandler<UserMappedToGroupEvent>
{
    private readonly ILogger<UserMappedToGroupEventHandler> _logger;

    public UserMappedToGroupEventHandler(ILogger<UserMappedToGroupEventHandler> logger) => _logger = logger;

    public Task Handle(UserMappedToGroupEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: User {UserId} mapped to group {GroupId}",
            notification.UserId, notification.GroupId);
        return Task.CompletedTask;
    }
}
