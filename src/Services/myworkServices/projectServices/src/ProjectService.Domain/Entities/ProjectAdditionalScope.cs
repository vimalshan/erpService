using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectAdditionalScope : BaseEntity
{
    public long ProjAdScopeId { get; set; }
    public long ProjAdScopeProjId { get; set; }
    public string ProjAdScopeDesc { get; set; } = null!;

    public virtual ProjectMain? Project { get; set; }
}
