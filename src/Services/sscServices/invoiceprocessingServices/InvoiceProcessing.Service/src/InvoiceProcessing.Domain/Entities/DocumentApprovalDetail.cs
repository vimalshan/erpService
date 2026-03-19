using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentApprovalDetail : BaseEntity
{
    public long SeqId { get; private set; }
    public long DocId { get; private set; }
    public long UserId { get; private set; }
    public string Status { get; private set; } = null!;
    public string? Remarks { get; private set; }
    public DateTime ApprovalDate { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
