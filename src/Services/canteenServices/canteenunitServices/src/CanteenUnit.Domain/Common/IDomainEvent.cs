using MediatR;

namespace CanteenUnit.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
