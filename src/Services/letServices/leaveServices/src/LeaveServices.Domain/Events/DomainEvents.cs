namespace LeaveServices.Domain.Events;

public abstract record DomainEventBase : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record LeaveRequestCreatedEvent(long ReqNum, string EmpUserId, DateTime ReqDate) : DomainEventBase;

public record LeaveEncashmentRequestedEvent(long EncashmentId, long EmpSysId, string LeaveType, int Days, decimal Amount) : DomainEventBase;

public record LeaveEncashmentStatusChangedEvent(long EncashmentId, char OldStatus, char NewStatus, long ModifiedBy) : DomainEventBase;

public record LossOfPayRecordedEvent(long LopId, long EmpSysId, int LopDays, DateOnly LopMonth) : DomainEventBase;

public record LeaveRequestDetailAddedEvent(long ReqNum, int SrlNum) : DomainEventBase;
