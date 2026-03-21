using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class TravelBatchSub : BaseEntity
{
    public string UnitCode { get; set; } = string.Empty;
    public decimal BatchNumber { get; set; }
    public decimal SerialNumber { get; set; }
    public decimal? BookingNumber { get; set; }
    public decimal? TicketCost { get; set; }
    public decimal? TicketAdjustment { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
    public string? CgstAmount { get; set; }
    public string? SgstAmount { get; set; }
    public string? IgstAmount { get; set; }

    public virtual TravelBatchMain BatchMain { get; set; } = null!;
}
