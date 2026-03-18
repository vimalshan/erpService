using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskMitigationAction : BaseEntity
{
    public long MitigationId { get; set; }
    public DateTime DueDate { get; set; }
    public char Status { get; set; }  // N/C/P/D
    public DateTime? RevisedDueDate { get; set; }
    public char ApprovalStatus { get; set; }  // E/P/A
    public string Comments { get; set; } = default!;
    public DateTime? CompletionDate { get; set; }

    private readonly List<RiskMitigationApproval> _approvals = new();
    public IReadOnlyCollection<RiskMitigationApproval> Approvals => _approvals.AsReadOnly();
}
