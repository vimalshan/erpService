using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskSelfAssessmentComment : BaseEntity
{
    public long AssessmentId { get; set; }
    public long RiskId { get; set; }
    public string Comments { get; set; } = default!;
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}
