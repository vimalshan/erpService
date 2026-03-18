using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectTypeCategoryMaster : AuditableEntity
{
    public long ProjCatId { get; set; }
    public string ProjCatName { get; set; } = null!;

    public virtual ICollection<ProjectTypeMaster> ProjectTypes { get; set; } = [];
}
