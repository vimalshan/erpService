using MediatR;

namespace ProxyModule.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
