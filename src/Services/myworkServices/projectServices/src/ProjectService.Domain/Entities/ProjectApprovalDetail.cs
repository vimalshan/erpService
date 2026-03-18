using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectApprovalDetail : BaseEntity
{
    public long ProjApprId { get; set; }
    public long ProjApprProjId { get; set; }
    public char ProjApprType { get; set; }
    public DateTime ProjApprSentOn { get; set; }
    public long ProjAppEmpSysId { get; set; }
    public DateTime ProjApprAppDate { get; set; }
    public char ProjApprStatus { get; set; }
    public string ProjApprRemarks { get; set; } = null!;
    public string ProjApprDropRemarks { get; set; } = null!;

    public virtual ProjectMain? Project { get; set; }
}
