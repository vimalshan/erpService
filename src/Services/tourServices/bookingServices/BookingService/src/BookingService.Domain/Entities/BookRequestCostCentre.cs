namespace BookingService.Domain.Entities;

public class BookRequestCostCentre
{
    public string BookCcId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string BusinessUnitCode { get; set; } = null!;
    public string CostCentreCode { get; set; } = null!;
    public string SubAccountCode { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string LocationSegment { get; set; } = null!;
    public string AllocationPercentage { get; set; } = null!;

    // Navigation
    public BookRequestMain Main { get; set; } = null!;
}
