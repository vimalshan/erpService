using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentCorrespondenceAttachment : BaseEntity
{
    public long AttId { get; private set; }
    public long CorrId { get; private set; }
    public string CorrStatus { get; private set; } = null!;
    public string FilePath { get; private set; } = null!;

    public DocumentCorrespondence Correspondence { get; private set; } = null!;
}
