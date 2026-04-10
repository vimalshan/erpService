using MediatR;

namespace TaskTransactional.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
