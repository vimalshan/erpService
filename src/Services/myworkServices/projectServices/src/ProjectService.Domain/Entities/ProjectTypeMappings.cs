using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectTypeDeliverableMap : AuditableEntity
{
    public long DelId { get; set; }
    public long DelProjTypeId { get; set; }
    public string DelDesc { get; set; } = null!;

    public virtual ProjectTypeMaster? ProjectType { get; set; }
}

public class ProjectTypeObjectiveMap : AuditableEntity
{
    public long ObjId { get; set; }
    public long ObjProjTypeId { get; set; }
    public string ObjDesc { get; set; } = null!;

    public virtual ProjectTypeMaster? ProjectType { get; set; }
}

public class ProjectTypeScopeMap : AuditableEntity
{
    public long ScopeId { get; set; }
    public long ScopeProjTypeId { get; set; }
    public string ScopeDesc { get; set; } = null!;

    public virtual ProjectTypeMaster? ProjectType { get; set; }
}

public class ProjectTypeFinYearSeq : BaseEntity
{
    public long ProjTypeId { get; set; }
    public int ProjTypeYear { get; set; }
    public long ProjTypeSeq { get; set; }
}
