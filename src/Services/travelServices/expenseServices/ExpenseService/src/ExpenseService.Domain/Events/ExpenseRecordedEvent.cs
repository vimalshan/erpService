using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Events;

public sealed record ExpenseRecordedEvent(
    long RequestNumber,
    long SerialNumber,
    decimal BudgetAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
