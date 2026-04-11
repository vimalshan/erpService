using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to TRAVEL_BATCHCC - Batch cost centre allocation
/// </summary>
public sealed class TravelBatchCostCentre : BaseEntity
{
    private TravelBatchCostCentre() { }

    public decimal CostNum { get; private set; }
    public decimal BatchSubNum { get; private set; }
    public string? UnitId { get; private set; }
    public string? SubAcc { get; private set; }
    public string? CostCode { get; private set; }
    public string? ProjectCode { get; private set; }
    public string? LocationCode { get; private set; }
    public string? IutaCode { get; private set; }

    public static TravelBatchCostCentre Create(
        decimal costNum, decimal batchSubNum, string? unitId = null,
        string? subAcc = null, string? costCode = null,
        string? projectCode = null, string? locationCode = null, string? iutaCode = null)
    {
        return new TravelBatchCostCentre
        {
            CostNum = costNum,
            BatchSubNum = batchSubNum,
            UnitId = unitId,
            SubAcc = subAcc,
            CostCode = costCode,
            ProjectCode = projectCode,
            LocationCode = locationCode,
            IutaCode = iutaCode
        };
    }
}
