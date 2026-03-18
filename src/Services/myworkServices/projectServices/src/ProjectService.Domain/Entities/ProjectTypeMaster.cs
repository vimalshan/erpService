using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entities;

public class ProjectTypeMaster : BaseEntity
{
    public long ProjTypeId { get; set; }
    public string ProjTypeName { get; set; } = null!;
    public string ProjTypeCode { get; set; } = null!;
    public decimal ProjTypeDepId { get; set; }
    public long ProjTypeCatId { get; set; }
    public decimal? ProjTypeModifiedBy { get; set; }
    public DateTime? ProjTypeModifiedOn { get; set; }

    public virtual ICollection<ProjectTypeDeliverableMap> DeliverableMaps { get; set; } = [];
    public virtual ICollection<ProjectTypeObjectiveMap> ObjectiveMaps { get; set; } = [];
    public virtual ICollection<ProjectTypeScopeMap> ScopeMaps { get; set; } = [];
    public virtual ICollection<ProjectTypeFunctionMap> FunctionMaps { get; set; } = [];
    public virtual ProjectTypeCategoryMaster? Category { get; set; }
}
