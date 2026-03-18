namespace GroupIncentiveService.Domain.Entities;

public class GroupIncentiveApproval : BaseEntity
{
    public long GrpIncAppId { get; private set; }
    public long GrpIncAppMainId { get; private set; }
    public long GrpIncAppApprover { get; private set; }
    public string GrpIncAppStatus { get; private set; } = default!;
    public string? GrpIncAppRemarks { get; private set; }
    public DateTime GrpIncAppApprovalDate { get; private set; }
    public long GrpIncAppLastModifiedBy { get; private set; }
    public DateTime GrpIncAppLastModifiedOn { get; private set; }

    public GroupIncentiveMain? Main { get; private set; }

    private GroupIncentiveApproval() { }

    public static GroupIncentiveApproval Create(long id, long mainId, long approver,
        string status, string? remarks, long modifiedBy)
    {
        return new GroupIncentiveApproval
        {
            GrpIncAppId = id,
            GrpIncAppMainId = mainId,
            GrpIncAppApprover = approver,
            GrpIncAppStatus = status,
            GrpIncAppRemarks = remarks?.Trim(),
            GrpIncAppApprovalDate = DateTime.UtcNow,
            GrpIncAppLastModifiedBy = modifiedBy,
            GrpIncAppLastModifiedOn = DateTime.UtcNow
        };
    }
}
