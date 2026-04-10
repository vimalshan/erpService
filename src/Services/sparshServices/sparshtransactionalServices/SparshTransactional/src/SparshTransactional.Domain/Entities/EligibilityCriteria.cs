using SparshTransactional.Domain.Common;

namespace SparshTransactional.Domain.Entities;

public class EligibilityCriteria : BaseEntity
{
    public long CriteriaId { get; set; }
    public long ScholarshipId { get; set; }
    public string CriteriaName { get; set; } = string.Empty;
    public string? CriteriaDescription { get; set; }
    public decimal? MinScore { get; set; }
    public decimal? MaxFamilyIncome { get; set; }
    public string EligibilityStatus { get; set; } = "A";
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    // Navigation
    public ScholarshipMaster Scholarship { get; set; } = null!;
}
