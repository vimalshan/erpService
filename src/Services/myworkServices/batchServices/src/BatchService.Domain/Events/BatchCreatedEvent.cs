using BatchService.Domain.Common;

namespace BatchService.Domain.Events;

public sealed record BatchCreatedEvent(long BatchId, int MonthNo) : IDomainEvent
{
    public Guid     EventId     { get; } = Guid.NewGuid();
    public DateTime OccurredOn  { get; } = DateTime.UtcNow;
    public string   EventType   => nameof(BatchCreatedEvent);
}
