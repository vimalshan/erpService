using ClubMembershipService.Domain.Common;

namespace ClubMembershipService.Domain.Events;

public sealed record MembershipCreatedEvent(long MembershipId, long ClubId, long MemberId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
