using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Events;

public record BatchCreatedDomainEvent(long BatchId, long CreatedBy, long LocationId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record BatchConfirmedDomainEvent(long BatchId, long ConfirmedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record BatchCancelledDomainEvent(long BatchId, long CancelledBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
