using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectHold : BaseEntity
{
    public long ProjHoldId { get; set; }
    public long ProjHoldProjId { get; set; }
    public char ProjHoldType { get; set; }
    public DateTime ProjHoldDate { get; set; }
    public string ProjHoldReason { get; set; } = null!;
    public long ProjHoldUpdatedBy { get; set; }
    public DateTime ProjHoldUpdatedOn { get; set; }

    public virtual ProjectMain? Project { get; set; }
}
