using AttendanceService.Domain.Common;
using AttendanceService.Domain.Events;

namespace AttendanceService.Domain.Entities;

public class AttendanceOvertime : BaseEntity
{
    public long OtEmpSysId { get; private set; }
    public DateTime OtDate { get; private set; }
    public decimal OtHours { get; private set; }
    public string OtType { get; private set; } = default!;
    public string OtApproved { get; private set; } = "N";
    public long OtLastModifiedBy { get; private set; }
    public DateTime OtLastModifiedOn { get; private set; }

    private AttendanceOvertime() { }

    public static AttendanceOvertime Create(long id, long empSysId, DateTime date,
        decimal hours, string otType, long createdBy)
        => new()
        {
            Id = id,
            OtEmpSysId = empSysId,
            OtDate = date,
            OtHours = hours,
            OtType = otType,
            OtApproved = "N",
            OtLastModifiedBy = createdBy,
            OtLastModifiedOn = DateTime.UtcNow
        };

    public void Approve(long approvedBy)
    {
        OtApproved = "Y";
        OtLastModifiedBy = approvedBy;
        OtLastModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new OvertimeApprovedEvent(Id, OtEmpSysId, OtDate, OtHours));
    }
}
