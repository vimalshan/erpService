using MediatR;

namespace WMTransactional.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
