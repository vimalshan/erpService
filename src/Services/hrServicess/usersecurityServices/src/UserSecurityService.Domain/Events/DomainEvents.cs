using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Events;

public sealed record UserCreatedEvent(string UserId, decimal EmpNum, string? EmpName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PasswordChangedEvent(string UserId, decimal EmpSysId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record UserAppMappedEvent(decimal EmpSysId, string AppCode, decimal HrRoleId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record UserDeactivatedEvent(string UserId, decimal EmpNum) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
