using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_ORACLEINVDET — Oracle invoice details</summary>
public class OracleInvoice : Entity<long>
{
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

    private OracleInvoice() { }

    public static OracleInvoice Create(long id, long docId, long invoiceId, decimal? voucherNo = null,
        string? invoiceType = null, string? invoiceNum = null, DateTime? invoiceDate = null,
        decimal? invoiceAmount = null)
    {
        return new OracleInvoice
        {
            Id = id,
            DocId = docId,
            InvoiceId = invoiceId,
            VoucherNo = voucherNo,
            InvoiceType = invoiceType,
            InvoiceNum = invoiceNum,
            InvoiceDate = invoiceDate,
            InvoiceAmount = invoiceAmount,
            InvoiceCreatedOn = DateTime.UtcNow
        };
    }

    public void UpdateStatus(string status) => InvoiceStatus = status;
}
