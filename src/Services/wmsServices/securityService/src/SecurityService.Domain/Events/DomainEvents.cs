using MediatR;

namespace SecurityService.Domain.Events;

public record UserCreatedEvent(int UserId, string Username, string Email) : INotification;
public record UserDeactivatedEvent(int UserId, string Username) : INotification;
public record UserLoggedInEvent(int UserId, string Username, DateTime LoginTime) : INotification;
public record RoleAssignedToUserEvent(int UserId, int RoleId) : INotification;
public record RoleRemovedFromUserEvent(int UserId, int RoleId) : INotification;
public record PermissionAssignedToRoleEvent(int RoleId, int PermissionId) : INotification;
