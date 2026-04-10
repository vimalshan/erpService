using SparshTransactional.Domain.Common;
using SparshTransactional.Domain.Events;

namespace SparshTransactional.Domain.Entities;

public class ScholarshipMaster : BaseEntity
{
    public long ScholarshipId { get; set; }
    public string ScholarshipName { get; set; } = string.Empty;
    public string? ScholarshipDescription { get; set; }
    public string? ScholarshipType { get; set; }
    public decimal? CoveragePercent { get; set; }
    public decimal? MaxAmount { get; set; }
    public string Status { get; set; } = "A";
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // Navigation
    public ICollection<EligibilityCriteria> EligibilityCriteria { get; set; } = [];
    public ICollection<ScholarshipApplication> Applications { get; set; } = [];

    public static ScholarshipMaster Create(string name, string? description, string? type,
        decimal? coveragePercent, decimal? maxAmount, long createdBy)
    {
        var scholarship = new ScholarshipMaster
        {
            ScholarshipName = name,
            ScholarshipDescription = description,
            ScholarshipType = type,
            CoveragePercent = coveragePercent,
            MaxAmount = maxAmount,
            Status = "A",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        scholarship.AddDomainEvent(new ScholarshipCreatedEvent(scholarship));
        return scholarship;
    }

    public void Deactivate(long updatedBy)
    {
        Status = "I";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ScholarshipDeactivatedEvent(this, updatedBy));
    }
}
