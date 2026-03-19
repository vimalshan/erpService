using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentDefectiveAttachment : BaseEntity
{
    public long DefAttId { get; private set; }
    public long AllocationId { get; private set; }
    public string FilePath { get; private set; } = null!;
}
