using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: COST_MAST</summary>
public sealed class CostMaster : AggregateRoot
{
    public long CostCode { get; private set; }
    public string CostName { get; private set; } = string.Empty;

    private CostMaster() { }

    public static CostMaster Create(long costCode, string costName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(costName);
        if (costCode <= 0) throw new ArgumentException("CostCode must be positive.");
        return new CostMaster { CostCode = costCode, CostName = costName.Trim() };
    }

    public void Update(string costName) => CostName = costName.Trim();
}
