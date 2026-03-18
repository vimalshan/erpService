using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>Leave Counter (maps to LET_COUNTERS)</summary>
public sealed class LeaveCounter : BaseEntity
{
    public string LtTypCod { get; private set; } = default!;
    public long? LtCntNum { get; private set; }
    public string? LtCntDes { get; private set; }

    private LeaveCounter() { }

    public static LeaveCounter Create(string typeCode, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeCode);
        return new LeaveCounter { LtTypCod = typeCode, LtCntNum = 0, LtCntDes = description };
    }

    public long Increment()
    {
        LtCntNum = (LtCntNum ?? 0) + 1;
        return LtCntNum.Value;
    }
}
