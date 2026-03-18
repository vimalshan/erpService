using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Entities;

public class AttendanceLeaveAdjust : BaseEntity
{
    public long LeaveAdjEmpSysId { get; private set; }
    public DateTime LeaveAdjDate { get; private set; }
    public string LeaveAdjType { get; private set; } = default!;
    public long LeaveAdjLastModifiedBy { get; private set; }
    public DateTime LeaveAdjLastModifiedOn { get; private set; }

    private AttendanceLeaveAdjust() { }

    public static AttendanceLeaveAdjust Create(long id, long empSysId, DateTime date,
        string leaveType, long createdBy)
        => new()
        {
            Id = id,
            LeaveAdjEmpSysId = empSysId,
            LeaveAdjDate = date,
            LeaveAdjType = leaveType,
            LeaveAdjLastModifiedBy = createdBy,
            LeaveAdjLastModifiedOn = DateTime.UtcNow
        };
}
