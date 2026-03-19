using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentCounter : BaseEntity
{
    public string BusinessUnitId { get; private set; } = null!;
    public long DocumentNo { get; private set; }
}
