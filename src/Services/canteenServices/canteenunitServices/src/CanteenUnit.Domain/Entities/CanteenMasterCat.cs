using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Entities;

/// <summary>Maps to CANTEEN_MASTER_CAT</summary>
public class CanteenMasterCat : BaseEntity
{
    public long? CnComCod { get; private set; }
    public long? CnCanNum { get; private set; }
    public char? CnGrdTyp { get; private set; }

    private CanteenMasterCat() { }

    public static CanteenMasterCat Create(long? comCode, long? canNum, char? gradeType)
        => new() { CnComCod = comCode, CnCanNum = canNum, CnGrdTyp = gradeType };
}
