using ReviewService.Domain.Common;
using ReviewService.Domain.Events;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to COURSE_FEEDMAIN table - primary course feedback entity.
/// </summary>
public class CourseFeedMain : BaseEntity
{
    public long FdCrsId { get; private set; }
    public string FdUsrId { get; private set; } = string.Empty;
    public DateTime FdRevDat { get; private set; }
    public string FdGenRem { get; private set; } = string.Empty;
    public long FdReqNum { get; private set; }
    public DateTime FdModDat { get; private set; }
    public long? FdSrlNum { get; private set; }

    public ICollection<CourseFeedSub> FeedSubs { get; private set; } = new List<CourseFeedSub>();

    private CourseFeedMain() { }

    public static CourseFeedMain Create(
        long courseId, string userId, DateTime reviewDate,
        string generalRemarks, long requestNum)
    {
        var feedback = new CourseFeedMain
        {
            FdCrsId = courseId,
            FdUsrId = userId,
            FdRevDat = reviewDate,
            FdGenRem = generalRemarks,
            FdReqNum = requestNum,
            FdModDat = DateTime.UtcNow
        };
        feedback.AddDomainEvent(new FeedbackSubmittedEvent(courseId, userId, reviewDate));
        return feedback;
    }

    public void Update(string generalRemarks)
    {
        FdGenRem = generalRemarks;
        FdModDat = DateTime.UtcNow;
    }
}
