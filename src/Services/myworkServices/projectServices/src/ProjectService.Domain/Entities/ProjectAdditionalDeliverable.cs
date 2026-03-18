using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectAdditionalDeliverable : BaseEntity
{
    public long ProjAdlDelId { get; set; }
    public long ProjAdlDelProjId { get; set; }
    public string ProjAdlDelDesc { get; set; } = null!;

    public virtual ProjectMain? Project { get; set; }
}
