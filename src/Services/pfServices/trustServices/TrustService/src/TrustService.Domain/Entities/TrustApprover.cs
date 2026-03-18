using TrustService.Domain.Common;

namespace TrustService.Domain.Entities;

public class TrustApprover : BaseEntity
{
    public long ApproverId { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public long ApproverSysId { get; private set; }
    public int ApproverLevel { get; private set; }
    public string ApproverType { get; private set; } = string.Empty;
    public DateTime EffDate { get; private set; }
    public DateTime? ClsDate { get; private set; }
    public string ApproverStatus { get; private set; } = "A";

    public TrustMaster Trust { get; private set; } = null!;

    private TrustApprover() { }

    public static TrustApprover Create(string trustCode, long approverSysId, int approverLevel,
        string approverType, DateTime effDate)
    {
        return new TrustApprover
        {
            TrustCode = trustCode,
            ApproverSysId = approverSysId,
            ApproverLevel = approverLevel,
            ApproverType = approverType,
            EffDate = effDate,
            ApproverStatus = "A"
        };
    }

    public void Deactivate(DateTime closureDate)
    {
        ClsDate = closureDate;
        ApproverStatus = "I";
    }
}
