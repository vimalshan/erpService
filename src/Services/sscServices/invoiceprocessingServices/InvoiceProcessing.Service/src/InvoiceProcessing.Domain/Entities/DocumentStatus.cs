using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentStatus : BaseEntity
{
    public string Flag { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string CompletedRemarks { get; private set; } = null!;
    public string PendingRemarks { get; private set; } = null!;
    public long? StageOrder { get; private set; }
    public string? CategoryGroup { get; private set; }
    public long? StageNo { get; private set; }
}
