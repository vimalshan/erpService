using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentCostCenter : BaseEntity
{
    public long CcId { get; private set; }
    public long DocId { get; private set; }
    public string BusinessUnitId { get; private set; } = null!;
    public string LocationCode { get; private set; } = null!;
    public string AccountCode { get; private set; } = null!;
    public string SubAccount { get; private set; } = null!;
    public string CostCenterCode { get; private set; } = null!;
    public string Product { get; private set; } = null!;
    public long Percentage { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
