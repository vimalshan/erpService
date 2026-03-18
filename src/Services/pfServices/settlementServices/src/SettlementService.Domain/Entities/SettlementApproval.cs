using SettlementService.Domain.Common;
using SettlementService.Domain.Enums;

namespace SettlementService.Domain.Entities;

public class SettlementApproval : BaseEntity
{
    public long AprId { get; private set; }
    public long SetNum { get; private set; }
    public int AprLevel { get; private set; }
    public long AprBySysId { get; private set; }
    public ApprovalStatus AprStatus { get; private set; }
    public string? AprRemarks { get; private set; }
    public DateTime AprDate { get; private set; }

    private SettlementApproval() { }

    public SettlementApproval(long setNum, int level, long approvedBy, string? remarks = null)
    {
        SetNum = setNum;
        AprLevel = level;
        AprBySysId = approvedBy;
        AprStatus = ApprovalStatus.Pending;
        AprRemarks = remarks;
        AprDate = DateTime.UtcNow;
    }

    public void Approve(string? remarks = null)
    {
        AprStatus = ApprovalStatus.Approved;
        AprRemarks = remarks;
        AprDate = DateTime.UtcNow;
    }

    public void Reject(string? remarks = null)
    {
        AprStatus = ApprovalStatus.Rejected;
        AprRemarks = remarks;
        AprDate = DateTime.UtcNow;
    }
}
