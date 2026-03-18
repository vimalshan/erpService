using LeaveServices.Domain.Common;
using LeaveServices.Domain.Events;
using LeaveServices.Domain.ValueObjects;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// LEAVE_DETAILS – Leave application. Acts as aggregate root.
/// </summary>
public class LeaveDetails : AggregateRoot
{
    public long    LeaveDetailId       { get; private set; }
    public long    LeaveEmpSysId       { get; private set; }
    public DateTime LeaveAppFrom       { get; private set; }
    public DateTime LeaveAppTo         { get; private set; }
    public string  LeaveAppType        { get; private set; } = default!;
    public long    LeaveId             { get; private set; }
    public int     LeaveTimeUnitId     { get; private set; }
    public string  LeaveAppStatus      { get; private set; } = default!;
    public decimal LeaveAppliedDays    { get; private set; }
    public string? LeaveReason         { get; private set; }
    public DateTime LeaveEnteredOn     { get; private set; }
    public long    LeaveEnteredBy      { get; private set; }
    public long    LeaveLastModifiedBy { get; private set; }
    public DateTime LeaveLastModifiedOn { get; private set; }

    // Navigation
    public LeaveMaster?                  LeaveMaster         { get; private set; }
    public ICollection<LeaveDetailsApproval> Approvals       { get; private set; } = new List<LeaveDetailsApproval>();

    private LeaveDetails() { }

    public static LeaveDetails Apply(
        long detailId, long empSysId, DateTime from, DateTime to,
        string appType, long leaveId, int timeUnitId, decimal appliedDays,
        string? reason, long appliedBy)
    {
        var entity = new LeaveDetails
        {
            LeaveDetailId        = detailId,
            Id                   = detailId,
            LeaveEmpSysId        = empSysId,
            LeaveAppFrom         = from,
            LeaveAppTo           = to,
            LeaveAppType         = appType,
            LeaveId              = leaveId,
            LeaveTimeUnitId      = timeUnitId,
            LeaveAppStatus       = "P",
            LeaveAppliedDays     = appliedDays,
            LeaveReason          = reason,
            LeaveEnteredOn       = DateTime.UtcNow,
            LeaveEnteredBy       = appliedBy,
            LeaveLastModifiedBy  = appliedBy,
            LeaveLastModifiedOn  = DateTime.UtcNow
        };
        entity.AddDomainEvent(new LeaveAppliedEvent(detailId, empSysId, leaveId, from, to, appliedDays));
        return entity;
    }

    public void Approve(long approvedBy)
    {
        LeaveAppStatus       = "Y";
        LeaveLastModifiedBy  = approvedBy;
        LeaveLastModifiedOn  = DateTime.UtcNow;
        AddDomainEvent(new LeaveApprovedEvent(LeaveDetailId, LeaveEmpSysId, approvedBy));
    }

    public void Reject(long approvedBy, string remarks)
    {
        LeaveAppStatus       = "R";
        LeaveLastModifiedBy  = approvedBy;
        LeaveLastModifiedOn  = DateTime.UtcNow;
        AddDomainEvent(new LeaveRejectedEvent(LeaveDetailId, LeaveEmpSysId, approvedBy, remarks));
    }

    public void Cancel(long cancelledBy)
    {
        if (LeaveAppStatus == "Y")
            throw new InvalidOperationException("An already approved leave cannot be directly cancelled; reject it first.");
        LeaveAppStatus       = "C";
        LeaveLastModifiedBy  = cancelledBy;
        LeaveLastModifiedOn  = DateTime.UtcNow;
        AddDomainEvent(new LeaveCancelledEvent(LeaveDetailId, LeaveEmpSysId, cancelledBy));
    }
}
