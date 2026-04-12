using MediatR;

namespace travelTransactionService.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
