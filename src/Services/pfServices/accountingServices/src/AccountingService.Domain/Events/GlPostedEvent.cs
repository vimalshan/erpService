using AccountingService.Domain.Common;
using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Events;

public sealed class GlPostedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public GlPosting GlPosting { get; }

    public GlPostedEvent(GlPosting glPosting)
        => GlPosting = glPosting;
}
