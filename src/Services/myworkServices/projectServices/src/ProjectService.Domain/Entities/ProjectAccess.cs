using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectAccess : BaseEntity
{
    public long ProjAccId { get; set; }
    public long ProjAccEmpSysId { get; set; }
    public char ProjAccType { get; set; }
    public long ProjAccDepId { get; set; }
}
