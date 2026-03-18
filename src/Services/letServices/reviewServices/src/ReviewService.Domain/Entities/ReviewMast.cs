using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to REVIEW_MAST table - review type master.
/// </summary>
public class ReviewMast : BaseEntity
{
    public string RvTypCod { get; private set; } = string.Empty;
    public string RvTypNam { get; private set; } = string.Empty;
    public string RvGrpCod { get; private set; } = string.Empty;

    private ReviewMast() { }

    public static ReviewMast Create(string typeCode, string typeName, string groupCode)
    {
        return new ReviewMast
        {
            RvTypCod = typeCode,
            RvTypNam = typeName,
            RvGrpCod = groupCode
        };
    }
}
