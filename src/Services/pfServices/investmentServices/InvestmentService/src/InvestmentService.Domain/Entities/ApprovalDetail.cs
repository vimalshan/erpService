using InvestmentService.Domain.Common;

namespace InvestmentService.Domain.Entities;

public class ApprovalDetail : BaseEntity
{
    public decimal ApprovalDetailId { get; set; }
    public long InvestmentId { get; set; }
    public decimal RefId { get; set; }
    public decimal ApprovalLevel { get; set; }
    public string Flag { get; set; } = null!;
    public decimal ApproverSysId { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? Remarks { get; set; }

    // Navigation
    public Investment Investment { get; set; } = null!;
}
