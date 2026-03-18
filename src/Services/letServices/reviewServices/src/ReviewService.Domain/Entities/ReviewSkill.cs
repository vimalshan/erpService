using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to REVIEW_SKILL table - skill-level assessments.
/// </summary>
public class ReviewSkill : BaseEntity
{
    public long SkReqId { get; private set; }
    public long SkSrlNum { get; private set; }
    public long SkActNum { get; private set; }
    public long SkRevNum { get; private set; }
    public long SkSklCod { get; private set; }
    public long SkLvlNum { get; private set; }
    public decimal SkRatPer { get; private set; }
    public string SkRemMrk { get; private set; } = string.Empty;

    private ReviewSkill() { }

    public static ReviewSkill Create(
        long reqId, long srlNum, long actNum, long revNum,
        long skillCode, long levelNum, decimal ratingPercent, string remarks)
    {
        return new ReviewSkill
        {
            SkReqId = reqId,
            SkSrlNum = srlNum,
            SkActNum = actNum,
            SkRevNum = revNum,
            SkSklCod = skillCode,
            SkLvlNum = levelNum,
            SkRatPer = ratingPercent,
            SkRemMrk = remarks
        };
    }
}
