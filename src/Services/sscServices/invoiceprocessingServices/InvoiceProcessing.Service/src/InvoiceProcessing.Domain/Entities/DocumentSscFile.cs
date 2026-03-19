using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentSscFile : BaseEntity
{
    public long FileId { get; private set; }
    public long DocId { get; private set; }
    public string FilePath { get; private set; } = null!;

    public DocumentDetail Document { get; private set; } = null!;
}
