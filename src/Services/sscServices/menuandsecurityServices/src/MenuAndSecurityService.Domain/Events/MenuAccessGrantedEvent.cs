using MediatR;

namespace MenuAndSecurityService.Domain.Events;

public sealed record MenuAccessGrantedEvent(long AccessId, long MenuId, long RoleId) : INotification;
