using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class PaymentTerm : BaseEntity
{
    public long TermId { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public long LastUpdatedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public long CreatedBy { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EnabledFlag { get; set; } = "Y";
    public decimal? DueCutoffDay { get; set; }
    public string? Description { get; set; }
}
