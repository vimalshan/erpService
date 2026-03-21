using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class ApInvoice : AggregateRoot
{
    public long InvoiceId { get; set; }
    public string? InvoiceNum { get; set; }
    public string? InvoiceTypeLookupCode { get; set; }
    public string? InvoiceDate { get; set; }
    public long? VendorId { get; set; }
    public long? VendorSiteId { get; set; }
    public string? InvoiceAmount { get; set; }
    public string? InvoiceCurrencyCode { get; set; }
    public string? ExchangeRate { get; set; }
    public string? ExchangeRateType { get; set; }
    public long? TermsId { get; set; }
    public string? PaymentMethodLookupCode { get; set; }
    public string? Description { get; set; }
    public string? LastUpdateDate { get; set; }
    public long? LastUpdatedBy { get; set; }
    public string? CreationDate { get; set; }
    public long? CreatedBy { get; set; }
    public decimal? OrgId { get; set; }
    public string? Status { get; set; }
    public long? AgencyId { get; set; }

    public virtual ICollection<ApInvoiceLine> InvoiceLines { get; set; } = new List<ApInvoiceLine>();
}
