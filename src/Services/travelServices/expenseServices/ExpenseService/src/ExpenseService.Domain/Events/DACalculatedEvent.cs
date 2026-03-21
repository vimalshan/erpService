using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Events;

public sealed record DACalculatedEvent(
    long RequestId,
    decimal TotalDAAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
