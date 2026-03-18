using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// Leave Request Detail (maps to LET_SUB)
/// </summary>
public sealed class LeaveRequestDetail : BaseEntity
{
    public long LsReqNum { get; private set; }
    public int LsSrlNum { get; private set; }
    public DateTime? LsModDat { get; private set; }
    public string? LsModUser { get; private set; }
    public char? LsPrefModdev { get; private set; }
    public string? LsActTaken { get; private set; }
    public int? LsCrsId { get; private set; }
    public string? LsTrnprgBhr { get; private set; }
    public string? LsImpbenPro { get; private set; }
    public string? LsMeasureCp { get; private set; }
    public string? LsMidyerRevnam { get; private set; }
    public string? LsMidyerRevdat { get; private set; }
    public string? LsMidyerRevrem { get; private set; }
    public string? LsAnnyerRevnam { get; private set; }
    public string? LsAnnyerRevdat { get; private set; }
    public string? LsAnnyerRevrem { get; private set; }
    public int? LsCompDev { get; private set; }
    public string? LsDomknowDev { get; private set; }
    public string? LsDomknowDevDet { get; private set; }
    public string? LsProcesDev { get; private set; }
    public string? LsProcesDevDet { get; private set; }
    public char? LsLetsubCode { get; private set; }
    public string? LsRevType { get; private set; }

    // Navigation back to aggregate root (EF only)
    public LeaveRequest? LeaveRequest { get; private set; }

    private LeaveRequestDetail() { }

    internal static LeaveRequestDetail Create(
        long reqNum,
        int srlNum,
        string? modUser,
        char? prefModDev,
        string? actTaken)
    {
        return new LeaveRequestDetail
        {
            LsReqNum = reqNum,
            LsSrlNum = srlNum,
            LsModDat = DateTime.UtcNow,
            LsModUser = modUser,
            LsPrefModdev = prefModDev,
            LsActTaken = actTaken
        };
    }

    public void UpdateReviewDetails(
        string? midYearRevName, string? midYearRevDate, string? midYearRevRem,
        string? annYearRevName, string? annYearRevDate, string? annYearRevRem)
    {
        LsMidyerRevnam = midYearRevName;
        LsMidyerRevdat = midYearRevDate;
        LsMidyerRevrem = midYearRevRem;
        LsAnnyerRevnam = annYearRevName;
        LsAnnyerRevdat = annYearRevDate;
        LsAnnyerRevrem = annYearRevRem;
        LsModDat = DateTime.UtcNow;
    }
}
