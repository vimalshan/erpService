using BatchService.Domain.Common;

namespace BatchService.Domain.Events;

public sealed record BatchStatusChangedEvent(long BatchId, char PreviousStatus, char NewStatus) : IDomainEvent
{
    public Guid     EventId    { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string   EventType  => nameof(BatchStatusChangedEvent);
}
