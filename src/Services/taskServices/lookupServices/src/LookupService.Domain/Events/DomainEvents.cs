using LookupService.Domain.Common;

namespace LookupService.Domain.Events;

public record LovCreatedEvent(long LovId, string LovType, string LovName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record LovUpdatedEvent(long LovId, string? OldName, string? NewName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ProcessCreatedEvent(decimal ProcessId, string ProcessName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ProcessUpdatedEvent(decimal ProcessId, string ProcessName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record AccessMasterCreatedEvent(decimal AccessMastId, decimal UnitLovMapId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
