using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentReportField : BaseEntity
{
    public long FieldId { get; private set; }
    public string? ColumnField { get; private set; }
    public string ColumnDisplayField { get; private set; } = null!;
}
