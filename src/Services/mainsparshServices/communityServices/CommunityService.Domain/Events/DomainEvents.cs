namespace CommunityService.Domain.Events;

using Interfaces;

public record CommunityCreatedEvent(long CommunityId, string CommunityCode, string CommunityName, long OwnerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(CommunityCreatedEvent);
}

public record CommunityUpdatedEvent(long CommunityId, string CommunityName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(CommunityUpdatedEvent);
}

public record CommunityDeletedEvent(long CommunityId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(CommunityDeletedEvent);
}

public record MemberAddedEvent(long CommunityId, long UserId, string MemberRole) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(MemberAddedEvent);
}

public record MemberRemovedEvent(long CommunityId, long UserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(MemberRemovedEvent);
}

public record MemberRoleChangedEvent(long CommunityId, long UserId, string NewRole) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(MemberRoleChangedEvent);
}
