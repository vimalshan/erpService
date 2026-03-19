using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class OracleDueDetail : BaseEntity
{
    public long DueId { get; private set; }
    public long DocId { get; private set; }
    public long? OrgId { get; private set; }
    public long InvoiceId { get; private set; }
    public decimal? VoucherNo { get; private set; }
    public string? DocumentId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public long? PaymentNum { get; private set; }
    public decimal? DueAmount { get; private set; }
    public decimal? DueCreatedBy { get; private set; }
    public DateTime? DueCreatedOn { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
