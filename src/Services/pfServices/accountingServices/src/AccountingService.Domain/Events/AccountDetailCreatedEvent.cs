using AccountingService.Domain.Common;
using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Events;

public sealed class AccountDetailCreatedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public AccountDetail AccountDetail { get; }

    public AccountDetailCreatedEvent(AccountDetail accountDetail)
        => AccountDetail = accountDetail;
}
