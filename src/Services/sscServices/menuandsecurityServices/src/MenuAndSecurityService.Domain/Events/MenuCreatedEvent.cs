using MediatR;

namespace MenuAndSecurityService.Domain.Events;

public sealed record MenuCreatedEvent(long MenuId, string MenuName) : INotification;
