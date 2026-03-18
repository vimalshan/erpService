using DispatchPlanning.Domain.Common;

namespace DispatchPlanning.Domain.Entities;

public class DispatchPlanBreakupItem : Entity
{
    public int BreakupItemId { get; private set; }
    public int SubGroupId { get; private set; }
    public int ProductId { get; private set; }
    public string BreakupItemDesc { get; private set; } = default!;
    public int UnitId { get; private set; }
    public int MainProductUnitsConFactor { get; private set; }
    public int BiDisplayOrder { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public string? ClosureDate { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public string? ModifiedDate { get; private set; }
    public decimal? PackageId { get; private set; }

    private DispatchPlanBreakupItem() { }

    public static DispatchPlanBreakupItem Create(int id, int subGroupId, int productId,
        string description, int unitId, int conversionFactor, int displayOrder,
        DateTime effectiveDate, decimal? packageId, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        return new DispatchPlanBreakupItem
        {
            BreakupItemId = id,
            SubGroupId = subGroupId,
            ProductId = productId,
            BreakupItemDesc = description,
            UnitId = unitId,
            MainProductUnitsConFactor = conversionFactor,
            BiDisplayOrder = displayOrder,
            EffectiveDate = effectiveDate,
            PackageId = packageId,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
    }

    public void Close(string closureDate, int modifiedBy)
    {
        ClosureDate = closureDate;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow.ToString("o");
    }
}
