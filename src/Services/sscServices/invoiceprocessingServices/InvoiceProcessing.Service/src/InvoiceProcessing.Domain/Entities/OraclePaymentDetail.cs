using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class OraclePaymentDetail : BaseEntity
{
    public long PayId { get; private set; }
    public long DocId { get; private set; }
    public long PaymentNum { get; private set; }
    public long InvoiceId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public decimal? GrossAmount { get; private set; }
    public decimal? AmountRemaining { get; private set; }
    public string? PaymentStatus { get; private set; }
    public string? PaymentMethod { get; private set; }
    public string? PrepaymentApplied { get; private set; }
    public decimal? PaymentCreatedBy { get; private set; }
    public DateTime? PaymentCreatedOn { get; private set; }
    public long? CheckId { get; private set; }
    public string? BnkStatus { get; private set; }
    public long? CheckNumber { get; private set; }
    public DateTime? CheckDate { get; private set; }
    public decimal? CheckAmount { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
