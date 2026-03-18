using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Entities;

/// <summary>Maps to CANTEEN_MASTER</summary>
public class CanteenMaster : BaseEntity
{
    public decimal CnComCod { get; private set; }       // CN_COM_COD PK
    public long CnCanNum { get; private set; }          // CN_CAN_NUM
    public DateTime? CnCanFro { get; private set; }     // CN_CAN_FRO
    public DateTime? CnCanTo { get; private set; }      // CN_CAN_TO
    public char? CnLivFlg { get; private set; }         // CN_LIV_FLG
    public decimal? CnEntUsr { get; private set; }      // CN_ENT_USR
    public DateTime? CnEntDat { get; private set; }     // CN_ENT_DAT
    public string? CnRemMrk { get; private set; }       // CN_REM_MRK

    public ICollection<CanteenMasterCat> Categories { get; private set; } = [];
    public ICollection<CanteenMasterGradeCat> GradeCategories { get; private set; } = [];

    private CanteenMaster() { }

    public static CanteenMaster Create(
        decimal comCode,
        long canNum,
        DateTime? fromDate,
        DateTime? toDate,
        char? liveFlag,
        decimal? enteredBy,
        string? remark)
    {
        return new CanteenMaster
        {
            CnComCod = comCode,
            CnCanNum = canNum,
            CnCanFro = fromDate,
            CnCanTo = toDate,
            CnLivFlg = liveFlag,
            CnEntUsr = enteredBy,
            CnEntDat = DateTime.UtcNow,
            CnRemMrk = remark
        };
    }

    public void SetLiveFlag(char flag) => CnLivFlg = flag;
}
