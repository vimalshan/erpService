using MediatR;

namespace MenuAndSecurityService.Domain.Events;

public sealed record MenuAccessRevokedEvent(long AccessId, long MenuId, long RoleId) : INotification;
