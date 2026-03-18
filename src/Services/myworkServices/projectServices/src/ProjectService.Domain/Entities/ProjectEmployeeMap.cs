using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectEmployeeMap : AuditableEntity
{
    public long ProjEmpId { get; set; }
    public long ProjEmpProjectId { get; set; }
    public long ProjEmpEmpSysId { get; set; }
    public DateTime ProjEmpEffDate { get; set; }
    public DateTime ProjEmpCloseDate { get; set; }

    public virtual ProjectMaster? Project { get; set; }
}
