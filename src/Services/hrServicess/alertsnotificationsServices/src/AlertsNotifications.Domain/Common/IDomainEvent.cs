using MediatR;

namespace AlertsNotifications.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
