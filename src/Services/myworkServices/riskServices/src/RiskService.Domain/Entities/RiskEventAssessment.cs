using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskEventAssessment
{
    public long Id { get; set; }
    public long AssessmentId { get; set; }
    public long RiskId { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
