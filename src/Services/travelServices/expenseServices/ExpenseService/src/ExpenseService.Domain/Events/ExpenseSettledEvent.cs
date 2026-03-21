using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Events;

public sealed record ExpenseSettledEvent(
    long RequestNumber,
    decimal SettlementAmount,
    decimal RefundAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
