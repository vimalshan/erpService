using ClubMembershipService.Domain.Common;

namespace ClubMembershipService.Domain.Events;

public sealed record ActivityRecordedEvent(long ActivityId, long ClubId, string ActivityName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
