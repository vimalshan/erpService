using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Events;

public record EnvelopeCreatedDomainEvent(long EnvelopeId, string EnvelopeType, long CreatedBy, long LocationId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record EnvelopeConfirmedDomainEvent(long EnvelopeId, long ConfirmedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record EnvelopeCancelledDomainEvent(long EnvelopeId, long CancelledBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
