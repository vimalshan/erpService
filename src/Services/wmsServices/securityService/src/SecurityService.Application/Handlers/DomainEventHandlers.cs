using MediatR;
using Microsoft.Extensions.Logging;
using SecurityService.Domain.Events;

namespace SecurityService.Application.Handlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;
    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(UserCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: User created - {UserId} ({Username})", notification.UserId, notification.Username);
        return Task.CompletedTask;
    }
}

public class UserDeactivatedEventHandler : INotificationHandler<UserDeactivatedEvent>
{
    private readonly ILogger<UserDeactivatedEventHandler> _logger;
    public UserDeactivatedEventHandler(ILogger<UserDeactivatedEventHandler> logger) => _logger = logger;

    public Task Handle(UserDeactivatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: User deactivated - {UserId} ({Username})", notification.UserId, notification.Username);
        return Task.CompletedTask;
    }
}

public class UserLoggedInEventHandler : INotificationHandler<UserLoggedInEvent>
{
    private readonly ILogger<UserLoggedInEventHandler> _logger;
    public UserLoggedInEventHandler(ILogger<UserLoggedInEventHandler> logger) => _logger = logger;

    public Task Handle(UserLoggedInEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: User logged in - {UserId} ({Username}) at {LoginTime}", notification.UserId, notification.Username, notification.LoginTime);
        return Task.CompletedTask;
    }
}

public class RoleAssignedToUserEventHandler : INotificationHandler<RoleAssignedToUserEvent>
{
    private readonly ILogger<RoleAssignedToUserEventHandler> _logger;
    public RoleAssignedToUserEventHandler(ILogger<RoleAssignedToUserEventHandler> logger) => _logger = logger;

    public Task Handle(RoleAssignedToUserEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Role {RoleId} assigned to User {UserId}", notification.RoleId, notification.UserId);
        return Task.CompletedTask;
    }
}

public class PermissionAssignedToRoleEventHandler : INotificationHandler<PermissionAssignedToRoleEvent>
{
    private readonly ILogger<PermissionAssignedToRoleEventHandler> _logger;
    public PermissionAssignedToRoleEventHandler(ILogger<PermissionAssignedToRoleEventHandler> logger) => _logger = logger;

    public Task Handle(PermissionAssignedToRoleEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Permission {PermissionId} assigned to Role {RoleId}", notification.PermissionId, notification.RoleId);
        return Task.CompletedTask;
    }
}
