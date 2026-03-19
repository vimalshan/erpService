using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentRevokeDetail : BaseEntity
{
    public long RevokeDetailId { get; private set; }
    public long DocId { get; private set; }
    public string RevokeRemarks { get; private set; } = null!;
    public string RevokeStatus { get; private set; } = null!;
    public long RevokedBy { get; private set; }
    public DateTime RevokedOn { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
