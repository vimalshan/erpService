using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectStatusHistory : BaseEntity
{
    public long ProjStatusId { get; set; }
    public long ProjStatusProjId { get; set; }
    public string? ProjStatusFile { get; set; }
    public DateTime ProjStatusDate { get; set; }
    public string ProjStatusRem { get; set; } = null!;
    public long ProjStatusRevNo { get; set; }
    public long ProjStatusVerNo { get; set; }

    public virtual ProjectMain? Project { get; set; }
}
