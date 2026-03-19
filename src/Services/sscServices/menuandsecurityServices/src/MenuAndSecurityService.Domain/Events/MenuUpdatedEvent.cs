using MediatR;

namespace MenuAndSecurityService.Domain.Events;

public sealed record MenuUpdatedEvent(long MenuId, string MenuName) : INotification;
