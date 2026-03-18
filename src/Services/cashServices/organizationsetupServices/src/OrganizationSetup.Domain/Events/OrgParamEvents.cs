using OrganizationSetup.Domain.Common;

namespace OrganizationSetup.Domain.Events;

public sealed record OrgParamUpdatedEvent(long ParamId, string ParamType, long ParamValue, long OrgId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record OrgParamDeletedEvent(long ParamId, long OrgId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
