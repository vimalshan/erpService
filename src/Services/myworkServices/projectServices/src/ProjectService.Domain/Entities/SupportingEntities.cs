using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectDepartment : AuditableEntity
{
    public decimal ProjDepId { get; set; }
    public string ProjDepName { get; set; } = null!;
    public string ProjDepCode { get; set; } = null!;
}

public class ProjectLocation : AuditableEntity
{
    public long LocId { get; set; }
    public string LocName { get; set; } = null!;
}

public class ProjectProcess : AuditableEntity
{
    public long ProcId { get; set; }
    public string ProcName { get; set; } = null!;
}

public class ProjectFunction : AuditableEntity
{
    public long ProjFuncId { get; set; }
    public string ProjFuncName { get; set; } = null!;

    public virtual ICollection<ProjectFunctionEmployeeMap> EmployeeMaps { get; set; } = [];
}

public class ProjectFunctionEmployeeMap : AuditableEntity
{
    public long ProjFuncEmpMapId { get; set; }
    public long ProjFuncEmpMapFuncId { get; set; }
    public long ProjFuncEmpMapEmpSysId { get; set; }
    public char ProjFuncEmpLiveFlag { get; set; }

    public virtual ProjectFunction? Function { get; set; }
}

public class ProjectTypeFunctionMap : AuditableEntity
{
    public long ProjTypeFuncMapId { get; set; }
    public long ProjTypeFuncTypeId { get; set; }
    public long ProjTypeFuncFuncId { get; set; }
    public long ProjTypeFuncAddlNo { get; set; }

    public virtual ProjectTypeMaster? ProjectType { get; set; }
    public virtual ProjectFunction? Function { get; set; }
}
