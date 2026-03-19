using ClubMembershipService.Domain.Common;

namespace ClubMembershipService.Domain.Events;

public sealed record ClubCreatedEvent(long ClubId, string ClubName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
