using AccountingService.Domain.Common;
using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Events;

public sealed class TransactionCreatedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public TransactionDetail Transaction { get; }

    public TransactionCreatedEvent(TransactionDetail transaction)
        => Transaction = transaction;
}
