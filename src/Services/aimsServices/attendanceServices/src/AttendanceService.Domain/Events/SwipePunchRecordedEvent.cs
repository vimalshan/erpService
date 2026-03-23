using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Events;

public sealed record SwipePunchRecordedEvent(
    long SwipeId,
    long EmpSysId,
    DateTime PunchTime,
    string PunchStatus) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
