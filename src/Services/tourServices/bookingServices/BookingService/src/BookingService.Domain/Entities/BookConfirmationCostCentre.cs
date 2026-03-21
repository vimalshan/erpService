namespace BookingService.Domain.Entities;

public class BookConfirmationCostCentre
{
    public long CcId { get; set; }
    public long MainId { get; set; }
    public string? BusinessUnitCode { get; set; }
    public string? CostCentreCode { get; set; }
    public string? SubAccountCode { get; set; }
    public string? ProductCode { get; set; }
    public string? LocationSegment { get; set; }
    public int? AllocationPercentage { get; set; }
}
