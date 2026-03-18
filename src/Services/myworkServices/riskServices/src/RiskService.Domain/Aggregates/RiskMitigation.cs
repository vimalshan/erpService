using RiskService.Domain.Common;
using RiskService.Domain.Entities;

namespace RiskService.Domain.Aggregates;

public class RiskMitigation : BaseEntity, IAggregateRoot
{
    public long RiskId { get; set; }
    public string Action { get; set; } = default!;
    public DateTime OriginalDueDate { get; set; }
    public DateTime DueDate { get; set; }
    public long OwnerId { get; set; }
    public long ReviewerId { get; set; }
    public char Status { get; set; }  // M/L/D
    public decimal? ProbabilityReduction { get; set; }
    public decimal? ImpactReduction { get; set; }
    public long? ApproverEmployeeSysId { get; set; }
    public string? Attachment { get; set; }

    private readonly List<RiskMitigationAction> _actions = new();
    public IReadOnlyCollection<RiskMitigationAction> Actions => _actions.AsReadOnly();

    public void AddAction(RiskMitigationAction action) => _actions.Add(action);

    public void MarkMitigated(long modifiedBy)
    {
        Status = 'M';
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Drop(long modifiedBy)
    {
        Status = 'D';
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
