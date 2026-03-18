using TrustService.Domain.Common;

namespace TrustService.Domain.Entities;

public class TrustRole : BaseEntity
{
    public string TrTrustCode { get; private set; } = string.Empty;
    public int TrRoleId { get; private set; }
    public string TrRoleCode { get; private set; } = string.Empty;
    public string TrUserId { get; private set; } = string.Empty;
    public long TrUserNo { get; private set; }
    public DateTime TrEffDate { get; private set; }
    public DateTime? TrClsDate { get; private set; }
    public string TrStatus { get; private set; } = "A";

    public TrustMaster Trust { get; private set; } = null!;

    private TrustRole() { }

    public static TrustRole Create(string trustCode, int roleId, string roleCode, string userId, long userNo)
    {
        return new TrustRole
        {
            TrTrustCode = trustCode,
            TrRoleId = roleId,
            TrRoleCode = roleCode,
            TrUserId = userId,
            TrUserNo = userNo,
            TrEffDate = DateTime.UtcNow,
            TrStatus = "A"
        };
    }

    public void Close(DateTime closureDate)
    {
        TrClsDate = closureDate;
        TrStatus = "C";
    }
}
