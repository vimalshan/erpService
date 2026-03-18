using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to REVIEW_SUB table.
/// </summary>
public class ReviewSub : BaseEntity
{
    public long? RevMainSrl { get; private set; }
    public long? RevRevNum { get; private set; }
    public char? RevNextStatus { get; private set; }
    public DateTime? RevDate { get; private set; }
    public long? RevBy { get; private set; }
    public string? RevRemMrk { get; private set; }
    public string? RevStatus { get; private set; }
    public string? RevProgRem { get; private set; }

    public ReviewMain? ReviewMain { get; private set; }

    private ReviewSub() { }

    public static ReviewSub Create(
        long mainSrl,
        long revNum,
        DateTime reviewDate,
        long reviewedBy,
        string status,
        string? remarks)
    {
        return new ReviewSub
        {
            RevMainSrl = mainSrl,
            RevRevNum = revNum,
            RevDate = reviewDate,
            RevBy = reviewedBy,
            RevStatus = status,
            RevRemMrk = remarks
        };
    }
}
