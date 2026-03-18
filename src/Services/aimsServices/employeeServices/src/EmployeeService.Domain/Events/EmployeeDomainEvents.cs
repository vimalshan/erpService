using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.Events;

public sealed record ApproverAssignedEvent(
    int ApproverId,
    long EmpSysId,
    long ApproverSysId,
    int Level
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record CalendarMappedEvent(
    long EmpCalId,
    long EmpSysId,
    int CalendarId
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record TimeInfoUpdatedEvent(
    long TimeInfoId,
    long EmpSysId,
    char AttFlag
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ShiftPatternChangedEvent(
    long EmpShiftId,
    long EmpSysId,
    string OrgPattern,
    string NewPattern
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
