using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class ApInvoiceLine : BaseEntity
{
    public long InvoiceId { get; set; }
    public long? InvoiceLineId { get; set; }
    public long LineNumber { get; set; }
    public string? LineTypeLookupCode { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? AccountingDate { get; set; }
    public string? Description { get; set; }
    public long? LastUpdatedBy { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? CreationDate { get; set; }
    public long? OrgId { get; set; }
    public string? AccountCode { get; set; }
    public string? ProjectCode { get; set; }
    public decimal? SgstAmt { get; set; }
    public decimal? CgstAmt { get; set; }
    public decimal? IgstAmt { get; set; }

    public virtual ApInvoice Invoice { get; set; } = null!;
}
