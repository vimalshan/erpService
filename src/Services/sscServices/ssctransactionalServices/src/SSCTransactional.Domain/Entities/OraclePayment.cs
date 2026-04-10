using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_ORACLEPAYDET — Oracle payment details</summary>
public class OraclePayment : Entity<long>
{
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
    public string? BankStatus { get; private set; }
    public long? CheckNumber { get; private set; }
    public DateTime? CheckDate { get; private set; }
    public decimal? CheckAmount { get; private set; }

    private OraclePayment() { }

    public static OraclePayment Create(long id, long docId, long paymentNum, long invoiceId,
        decimal? grossAmount = null, DateTime? dueDate = null)
    {
        return new OraclePayment
        {
            Id = id,
            DocId = docId,
            PaymentNum = paymentNum,
            InvoiceId = invoiceId,
            GrossAmount = grossAmount,
            DueDate = dueDate,
            PaymentCreatedOn = DateTime.UtcNow
        };
    }

    public void UpdateStatus(string status) => PaymentStatus = status;
}
