using AttendanceService.Domain.Common;
using AttendanceService.Domain.Events;
using AttendanceService.Domain.ValueObjects;

namespace AttendanceService.Domain.Entities;

public class SwipeRawPunch : BaseEntity
{
    public long SwipeEmpSysId { get; private set; }
    public DateTime SwipePunchTime { get; private set; }
    public string SwipeGateNo { get; private set; } = default!;
    public PunchStatus SwipePunchStatus { get; private set; } = default!;
    public string? SwipePullStatus { get; private set; }
    public string? SwipeVerified { get; private set; }
    public long? SwipeLastModifiedBy { get; private set; }
    public DateTime? SwipeLastModifiedOn { get; private set; }

    private SwipeRawPunch() { }

    public static SwipeRawPunch Create(long id, long empSysId, DateTime punchTime,
        string gateNo, string punchStatusCode)
    {
        var punch = new SwipeRawPunch
        {
            Id = id,
            SwipeEmpSysId = empSysId,
            SwipePunchTime = punchTime,
            SwipeGateNo = gateNo,
            SwipePunchStatus = PunchStatus.From(punchStatusCode),
            SwipePullStatus = "A"
        };
        punch.AddDomainEvent(new SwipePunchRecordedEvent(id, empSysId, punchTime, punchStatusCode));
        return punch;
    }

    public void MarkVerified(long modifiedBy)
    {
        SwipeVerified = "Y";
        SwipeLastModifiedBy = modifiedBy;
        SwipeLastModifiedOn = DateTime.UtcNow;
    }
}
