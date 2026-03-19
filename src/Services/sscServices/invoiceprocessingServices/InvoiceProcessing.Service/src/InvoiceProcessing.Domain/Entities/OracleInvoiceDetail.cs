using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class OracleInvoiceDetail : BaseEntity
{
    public long InvId { get; private set; }
    public long DocId { get; private set; }
    public decimal? VoucherNo { get; private set; }
    public string? InvoiceType { get; private set; }
    public long? VendorId { get; private set; }
    public long? VendorSiteId { get; private set; }
    public string? InvoiceNum { get; private set; }
    public DateTime? InvoiceDate { get; private set; }
    public decimal? InvoiceAmount { get; private set; }
    public long InvoiceId { get; private set; }
    public string? InvoiceStatus { get; private set; }
    public DateTime? InvoiceCreatedOn { get; private set; }
    public decimal? InvoiceCreatedBy { get; private set; }
    public string? PaymentMethodCode { get; private set; }
    public DateTime? AccountingDate { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
