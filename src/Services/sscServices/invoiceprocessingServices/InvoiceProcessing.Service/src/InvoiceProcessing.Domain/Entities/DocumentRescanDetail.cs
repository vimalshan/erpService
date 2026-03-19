using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentRescanDetail : BaseEntity
{
    public long RescanId { get; private set; }
    public long DocId { get; private set; }
    public long AllocationId { get; private set; }
    public string Status { get; private set; } = null!;
    public DateTime RescanDate { get; private set; }
    public string RescanTo { get; private set; } = null!;
    public string Remarks { get; private set; } = null!;
    public DateTime? CompletedOn { get; private set; }
    public long? CompletedBy { get; private set; }
    public string? CompletionRemarks { get; private set; }
    public string? FilePath { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
