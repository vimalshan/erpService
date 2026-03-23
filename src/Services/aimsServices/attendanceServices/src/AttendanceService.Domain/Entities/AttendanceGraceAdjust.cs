using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Entities;

public class AttendanceGraceAdjust : BaseEntity
{
    public long GraceEmpSysId { get; private set; }
    public DateTime GraceDate { get; private set; }
    public int GraceMinutes { get; private set; }
    public long GraceLastModifiedBy { get; private set; }
    public DateTime GraceLastModifiedOn { get; private set; }

    private AttendanceGraceAdjust() { }

    public static AttendanceGraceAdjust Create(long id, long empSysId, DateTime date,
        int minutes, long createdBy)
        => new()
        {
            Id = id,
            GraceEmpSysId = empSysId,
            GraceDate = date,
            GraceMinutes = minutes,
            GraceLastModifiedBy = createdBy,
            GraceLastModifiedOn = DateTime.UtcNow
        };
}
