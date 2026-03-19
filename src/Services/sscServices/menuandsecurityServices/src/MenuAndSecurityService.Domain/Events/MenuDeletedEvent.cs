using MediatR;

namespace MenuAndSecurityService.Domain.Events;

public sealed record MenuDeletedEvent(long MenuId) : INotification;
