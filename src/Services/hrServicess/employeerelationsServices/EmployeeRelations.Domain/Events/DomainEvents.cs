using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Events;

public sealed record DisciplinaryCaseCreatedEvent(long MainId, long UnitId, DateTime Date) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DisciplinaryActionAddedEvent(long ActionId, long MainId, long EmpSysId, DateTime ActionDate) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DisciplinaryActionApprovedEvent(long ActionId, long ApprovedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record EwsCreatedEvent(long EwsId, long EmpSysId, int PeriodNo) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record EwsHrInputRecordedEvent(long EwsId, long HrEntryBy, string Flag) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record EwsCompletedEvent(long EwsId, long EmpSysId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SurveyCreatedEvent(long SurveyId, string Name, DateTime StartDate) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SurveyClosedEvent(long SurveyId, DateTime ClosureDate) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SurveyResponseSubmittedEvent(long ResponseId, long SurveyId, long EmpSysId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
