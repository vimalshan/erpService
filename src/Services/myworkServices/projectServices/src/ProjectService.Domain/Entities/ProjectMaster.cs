using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectMaster : AuditableEntity
{
    public long ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    public long ProjectCategoryId { get; set; }
    public DateTime ProjectEffDate { get; set; }
    public DateTime? ProjectClsDate { get; set; }
    public decimal ProjectTeamId { get; set; }
    public char ProjectListAll { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectEmployeeMap> EmployeeMaps { get; set; } = [];
    public virtual ProjectCategoryMaster? Category { get; set; }
}
