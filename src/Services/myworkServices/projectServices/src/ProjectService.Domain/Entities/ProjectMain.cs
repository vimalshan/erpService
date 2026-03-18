using ProjectService.Domain.Common;
using ProjectService.Domain.Enums;

namespace ProjectService.Domain.Entities;

public class ProjectMain : BaseEntity
{
    public long ProjId { get; set; }
    public string ProjName { get; set; } = null!;
    public decimal ProjCharterNo { get; set; }
    public long ProjLeaderId { get; set; }
    public long ProjTypeId { get; set; }
    public long ProjLocId { get; set; }
    public long ProjProcessId { get; set; }
    public DateTime ProjStartDate { get; set; }
    public DateTime ProjEndDate { get; set; }
    public DateTime ProjEstEndDate { get; set; }
    public char ProjStatus { get; set; }
    public int ProjRevNo { get; set; }
    public int ProjVerNo { get; set; }
    public long? ProjObjId { get; set; }
    public string? ProjObjDesc { get; set; }
    public string? ProjTargetProd { get; set; }
    public string? ProjTargetProdRem { get; set; }
    public string? ProjTargetSpecFile { get; set; }
    public string? ProjTargetSpecRem { get; set; }
    public string? ProjTargetYieldFile { get; set; }
    public string? ProjTargetYieldRem { get; set; }
    public string? ProjNotes { get; set; }
    public string? ProjActualProd { get; set; }
    public string? ProjActualProdRem { get; set; }
    public string? ProjActualSpecFile { get; set; }
    public string? ProjActualSpecRem { get; set; }
    public string? ProjActualYieldFile { get; set; }
    public string? ProjActualYieldRem { get; set; }
    public DateTime? ProjClsDate { get; set; }
    public DateTime? ProjLastModifiedOn { get; set; }
    public decimal? ProjAppEmpSysId { get; set; }
    public string? ProjPlanFile { get; set; }
    public string? ProjTargetLbl1 { get; set; }
    public string? ProjTargetLbl2 { get; set; }
    public string? ProjTargetLbl3 { get; set; }
    public string? ProjPptxFile { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectMember> Members { get; set; } = [];
    public virtual ICollection<ProjectScope> Scopes { get; set; } = [];
    public virtual ICollection<ProjectStatusHistory> StatusHistory { get; set; } = [];
    public virtual ICollection<ProjectAdditionalDeliverable> AdditionalDeliverables { get; set; } = [];
    public virtual ICollection<ProjectAdditionalScope> AdditionalScopes { get; set; } = [];
    public virtual ICollection<ProjectApprovalDetail> ApprovalDetails { get; set; } = [];
    public virtual ICollection<ProjectDeliverable> Deliverables { get; set; } = [];
    public virtual ICollection<ProjectHold> Holds { get; set; } = [];

    public virtual ProjectTypeMaster? ProjectType { get; set; }
    public virtual ProjectLocation? Location { get; set; }
    public virtual ProjectProcess? Process { get; set; }
}
