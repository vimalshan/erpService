using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentDuplicateCheck : BaseEntity
{
    public string? DocId { get; private set; }
}
