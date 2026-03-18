using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// LEAVE_DETAILSAPR – Approval history record for a leave application.
/// </summary>
public class LeaveDetailsApproval : AggregateRoot
{
    public long    LeaveAprId             { get; private set; }
    public long    LeaveAprDetailId       { get; private set; }
    public string  LeaveAprApproveStatus  { get; private set; } = default!;
    public string? LeaveAprRemarks        { get; private set; }
    public DateTime LeaveAprApprovedOn    { get; private set; }
    public long    LeaveAprApprovedBy     { get; private set; }
    public long    LeaveAprLastModifiedBy { get; private set; }
    public DateTime LeaveAprLastModifiedOn { get; private set; }

    public LeaveDetails? LeaveDetails { get; private set; }

    private LeaveDetailsApproval() { }

    public static LeaveDetailsApproval Create(
        long aprId, long detailId, string status, string? remarks, long approvedBy)
    {
        return new LeaveDetailsApproval
        {
            LeaveAprId              = aprId,
            Id                      = aprId,
            LeaveAprDetailId        = detailId,
            LeaveAprApproveStatus   = status,
            LeaveAprRemarks         = remarks,
            LeaveAprApprovedOn      = DateTime.UtcNow,
            LeaveAprApprovedBy      = approvedBy,
            LeaveAprLastModifiedBy  = approvedBy,
            LeaveAprLastModifiedOn  = DateTime.UtcNow
        };
    }
}
