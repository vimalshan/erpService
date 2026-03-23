using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Events;

public sealed record OvertimeApprovedEvent(
    long OvertimeId,
    long EmpSysId,
    DateTime OtDate,
    decimal Hours) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
