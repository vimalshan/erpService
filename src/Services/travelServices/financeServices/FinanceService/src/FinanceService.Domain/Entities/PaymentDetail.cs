using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class PaymentDetail : BaseEntity
{
    public long? Sno { get; set; }
    public long? BookNo { get; set; }
    public string? Vendor { get; set; }
    public decimal? TsTicketCost { get; set; }
    public decimal? TsTicketAdj { get; set; }
    public decimal? TsBaseStax { get; set; }
    public decimal? TsApproveAmt { get; set; }
    public string? TsStatus { get; set; }
    public string? TmInvNum { get; set; }
    public string? TmInvDat { get; set; }
    public decimal? TmInvAmt { get; set; }
    public decimal? TmTotApprAmt { get; set; }
    public decimal? TmTotal { get; set; }
    public long? TmJvNo { get; set; }
    public string? TmPaymentTerms { get; set; }
    public decimal? ServiceTax { get; set; }
}
