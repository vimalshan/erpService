using MediatR;

namespace PFTransactionalService.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
