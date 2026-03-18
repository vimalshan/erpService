using InvestmentService.Domain.Common;

namespace InvestmentService.Domain.Entities;

public class SaleDetail : BaseEntity
{
    public long SaleNo { get; set; }
    public long InvNo { get; set; }
    public string SaleType { get; set; } = null!;
    public DateTime SaleDate { get; set; }
    public decimal InterestAdjusted { get; set; }
    public decimal SalePremium { get; set; }
    public decimal SaleValue { get; set; }
    public int SaleTransactionId { get; set; }
    public string? Remarks { get; set; }
    public long EnteredBy { get; set; }
    public DateTime EnteredOn { get; set; }
    public long LastModBy { get; set; }
    public DateTime LastModOn { get; set; }

    // Navigation
    public Investment Investment { get; set; } = null!;
}
