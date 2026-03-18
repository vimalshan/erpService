using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectScope : BaseEntity
{
    public long ProjScopeId { get; set; }
    public long ProjScopeProjId { get; set; }
    public long ProjScopeScopeId { get; set; }

    public virtual ProjectMain? Project { get; set; }
}
