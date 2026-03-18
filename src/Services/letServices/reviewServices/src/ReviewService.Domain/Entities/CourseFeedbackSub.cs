using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to COURSE_FEEDBACKSUB table.
/// </summary>
public class CourseFeedbackSub : BaseEntity
{
    public long? FdFedNum { get; private set; }
    public long? FdFedTyp { get; private set; }
    public long? FdRatNum { get; private set; }
    public string? FdRemMrk { get; private set; }

    public CourseFeedbackMain? FeedbackMain { get; private set; }

    private CourseFeedbackSub() { }

    public static CourseFeedbackSub Create(
        long fedNum, long fedType, long ratingNum, string? remarks)
    {
        return new CourseFeedbackSub
        {
            FdFedNum = fedNum,
            FdFedTyp = fedType,
            FdRatNum = ratingNum,
            FdRemMrk = remarks
        };
    }
}
