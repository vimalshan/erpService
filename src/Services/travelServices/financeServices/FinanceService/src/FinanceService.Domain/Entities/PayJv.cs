using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class PayJv : BaseEntity
{
    public string CompanyCode { get; set; } = string.Empty;
    public DateTime FinancialYear { get; set; }
    public long DocumentNumber { get; set; }
    public long SerialNumber { get; set; }
    public long? PayBatchNo { get; set; }
    public DateTime? PayDate { get; set; }
    public string? AccountCode { get; set; }
    public decimal? TransactionAmount { get; set; }
    public string? Narration { get; set; }
    public string? PostingFlag { get; set; }
    public DateTime? EnteredOn { get; set; }
    public DateTime? CancelledOn { get; set; }
    public string? EnteredBy { get; set; }
}
