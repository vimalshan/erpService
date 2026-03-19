namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to PULLMATRIX_DET table - Pull Matrix Details
/// </summary>
public class PullMatrixDetail : BaseEntity
{
    public long MatId { get; private set; }
    public long MatUnitId { get; private set; }
    public string MatPayBy { get; private set; } = string.Empty;
    public char MatFlag { get; private set; }
    public long MatMainCat { get; private set; }
    public long MatEmpSysId { get; private set; }
    public long MatMaxNos { get; private set; }
    public long MatCreatedBy { get; private set; }
    public DateTime MatCreatedOn { get; private set; }
    public long MatModifiedBy { get; private set; }
    public DateTime MatModifiedOn { get; private set; }

    private PullMatrixDetail() { }

    public static PullMatrixDetail Create(long matId, long unitId, string payBy, char flag,
        long mainCat, long empSysId, long maxNos, long createdBy)
    {
        return new PullMatrixDetail
        {
            MatId = matId,
            MatUnitId = unitId,
            MatPayBy = payBy,
            MatFlag = flag,
            MatMainCat = mainCat,
            MatEmpSysId = empSysId,
            MatMaxNos = maxNos,
            MatCreatedBy = createdBy,
            MatCreatedOn = DateTime.UtcNow,
            MatModifiedBy = createdBy,
            MatModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string payBy, char flag, long mainCat, long empSysId, long maxNos, long modifiedBy)
    {
        MatPayBy = payBy;
        MatFlag = flag;
        MatMainCat = mainCat;
        MatEmpSysId = empSysId;
        MatMaxNos = maxNos;
        MatModifiedBy = modifiedBy;
        MatModifiedOn = DateTime.UtcNow;
    }
}
