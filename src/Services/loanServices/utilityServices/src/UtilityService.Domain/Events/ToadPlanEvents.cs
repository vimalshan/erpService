using UtilityService.Domain.Common;

namespace UtilityService.Domain.Events;

public sealed record ToadPlanCreatedEvent(string StatementId, string? Username) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ToadPlanUpdatedEvent(string StatementId, string? Username) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ToadPlanDeletedEvent(string StatementId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
