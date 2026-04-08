using MediatR;

namespace SciTransactional.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
