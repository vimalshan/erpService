using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectCategoryMaster : AuditableEntity
{
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public long CategoryTeamId { get; set; }

    public virtual ICollection<ProjectMaster> Projects { get; set; } = [];
}
