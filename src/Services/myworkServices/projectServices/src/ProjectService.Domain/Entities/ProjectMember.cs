using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectMember : BaseEntity
{
    public long ProjMemId { get; set; }
    public long ProjMemProjId { get; set; }
    public long ProjMemFuncId { get; set; }
    public long ProjMemEmpSysId { get; set; }

    public virtual ProjectMain? Project { get; set; }
    public virtual ProjectFunction? Function { get; set; }
}
