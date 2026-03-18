using TimesheetService.Domain.Common;

namespace TimesheetService.Domain.Events;

public sealed record TimesheetCreatedEvent(long TimesheetId, long EmployeeId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record TimesheetSubmittedEvent(long TimesheetId, long EmployeeId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record TimesheetApprovedEvent(long TimesheetId, long EmployeeId, long ApproverId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record TimesheetRejectedEvent(long TimesheetId, long EmployeeId, string RejectionReason) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
