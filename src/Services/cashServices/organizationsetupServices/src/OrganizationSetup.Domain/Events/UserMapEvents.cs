using OrganizationSetup.Domain.Common;

namespace OrganizationSetup.Domain.Events;

public sealed record UserMappedToRoleEvent(long MapId, long RoleId, long EmpSysId, long OrgId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record UserUnmappedFromRoleEvent(long MapId, long RoleId, long EmpSysId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
