using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_ORACLEDUEDET — Oracle due details</summary>
public class OracleDueDetail : Entity<long>
{
    public long DocId { get; private set; }
    public long? OrgId { get; private set; }
    public long InvoiceId { get; private set; }
    public decimal? VoucherNo { get; private set; }
    public string? DocumentId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public long? PaymentNum { get; private set; }
    public decimal? DueAmount { get; private set; }
    public decimal? CreatedBy { get; private set; }
    public DateTime? CreatedOn { get; private set; }

    private OracleDueDetail() { }

    public static OracleDueDetail Create(long id, long docId, long invoiceId,
        decimal? voucherNo = null, DateTime? dueDate = null, decimal? dueAmount = null)
    {
        return new OracleDueDetail
        {
            Id = id,
            DocId = docId,
            InvoiceId = invoiceId,
            VoucherNo = voucherNo,
            DueDate = dueDate,
            DueAmount = dueAmount,
            CreatedOn = DateTime.UtcNow
        };
    }
}
