using InvestmentService.Domain.Common;

namespace InvestmentService.Domain.Entities;

public class CallDetail : BaseEntity
{
    public long CallDetailId { get; set; }
    public long InvNo { get; set; }
    public DateTime CallDate { get; set; }
    public decimal CallAmount { get; set; }
    public string ConfirmStatus { get; set; } = null!;
    public string InterestRevFlag { get; set; } = null!;
    public decimal? RevisedInterestRate { get; set; }
    public long? SaleRefId { get; set; }
    public decimal? LastModBy { get; set; }
    public DateTime? LastModOn { get; set; }
    public long? SlNo { get; set; }

    // Navigation
    public Investment Investment { get; set; } = null!;
}
