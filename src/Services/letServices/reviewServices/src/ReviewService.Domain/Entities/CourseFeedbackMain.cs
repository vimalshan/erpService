using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to COURSE_FEEDBACKMAIN table.
/// </summary>
public class CourseFeedbackMain : BaseEntity
{
    public long? FdFedNum { get; private set; }
    public long? FdNomNum { get; private set; }
    public char? FdStsCod { get; private set; }
    public DateTime? FdFedDat { get; private set; }
    public DateTime? FdModDat { get; private set; }
    public long? FdFinRat { get; private set; }
    public string? FdRemLin1 { get; private set; }
    public string? FdRemLin2 { get; private set; }
    public string? FdRemLin3 { get; private set; }
    public decimal? FdRevSrl { get; private set; }
    public string? FdCancelRem { get; private set; }
    public long? FdReqNum { get; private set; }
    public string? FdRemLin9 { get; private set; }
    public string? FdRemLin4 { get; private set; }
    public string? FdRemLin5 { get; private set; }
    public long? FdRemLin6 { get; private set; }
    public string? FdRemLin7 { get; private set; }
    public string? FdRemLin8 { get; private set; }

    public ICollection<CourseFeedbackSub> FeedbackSubs { get; private set; } = new List<CourseFeedbackSub>();

    private CourseFeedbackMain() { }

    public static CourseFeedbackMain Create(
        long? fedNum, long? nomNum, char statusCode,
        DateTime feedbackDate, long? finalRating, long? reqNum)
    {
        return new CourseFeedbackMain
        {
            FdFedNum = fedNum,
            FdNomNum = nomNum,
            FdStsCod = statusCode,
            FdFedDat = feedbackDate,
            FdModDat = DateTime.UtcNow,
            FdFinRat = finalRating,
            FdReqNum = reqNum
        };
    }
}
