using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentAttachment : BaseEntity
{
    public long AttachId { get; private set; }
    public long DocId { get; private set; }
    public string FilePath { get; private set; } = null!;

    public DocumentDetail Document { get; private set; } = null!;
}
