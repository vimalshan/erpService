using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectDeliverable : BaseEntity
{
    public long ProjDelId { get; set; }
    public long ProjDelProjId { get; set; }
    public long ProjDelDelId { get; set; }

    public virtual ProjectMain? Project { get; set; }
}
