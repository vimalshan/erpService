using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class PayOtherDetail : BaseEntity
{
    public long CompanyCode { get; set; }
    public long TransactionNumber { get; set; }
    public long? PayBatchNo { get; set; }
    public long? VendorCode { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? PayMode { get; set; }
    public decimal? PayAmount { get; set; }
    public DateTime? ChequeDate { get; set; }
    public long? ChequeNumber { get; set; }
    public DateTime? PayDate { get; set; }
    public string? Remarks { get; set; }
    public string? StatusCode { get; set; }
}
