using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanCostCentre : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string BusinessUnit { get; private set; } = string.Empty;
    public string CostCentreCode { get; private set; } = string.Empty;
    public string SubAccountCode { get; private set; } = string.Empty;
    public string ProductCode { get; private set; } = string.Empty;
    public string LocationSegment { get; private set; } = string.Empty;
    public decimal AllocationPercentage { get; private set; }
    public bool IsDefault { get; private set; }

    protected TourPlanCostCentre() { }

    public static TourPlanCostCentre Create(
        string id, string tourPlanId, string businessUnit, string costCentreCode,
        string subAccountCode, string productCode, string locationSegment,
        decimal allocationPercentage, bool isDefault = false)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            BusinessUnit = businessUnit,
            CostCentreCode = costCentreCode,
            SubAccountCode = subAccountCode,
            ProductCode = productCode,
            LocationSegment = locationSegment,
            AllocationPercentage = allocationPercentage,
            IsDefault = isDefault
        };
}
