using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Entities;

/// <summary>Maps to CANTEEN_MASTER_GRADECAT</summary>
public class CanteenMasterGradeCat : BaseEntity
{
    public long? CnCanSeq { get; private set; }
    public decimal? CnComCod { get; private set; }
    public long? CnCanNum { get; private set; }
    public DateTime? CnCanFro { get; private set; }
    public DateTime? CnCanTo { get; private set; }
    public char? CnLivFlg { get; private set; }
    public string? CnGrdCat { get; private set; }

    private CanteenMasterGradeCat() { }

    public static CanteenMasterGradeCat Create(
        long? seq, decimal? comCode, long? canNum,
        DateTime? from, DateTime? to, char? liveFlag, string? gradeCategory)
        => new()
        {
            CnCanSeq = seq, CnComCod = comCode, CnCanNum = canNum,
            CnCanFro = from, CnCanTo = to, CnLivFlg = liveFlag, CnGrdCat = gradeCategory
        };
}
