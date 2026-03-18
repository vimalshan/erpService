using ReviewService.Domain.Common;
using ReviewService.Domain.Entities;
using ReviewService.Domain.Events;

namespace ReviewService.Domain.Aggregates;

/// <summary>
/// Aggregate root for the Feedback aggregate (COURSE_FEEDMAIN + COURSE_FEEDSUB).
/// </summary>
public class FeedbackAggregate : AggregateRoot
{
    public CourseFeedMain FeedMain { get; private set; } = null!;
    public IReadOnlyCollection<CourseFeedSub> FeedSubs => _feedSubs.AsReadOnly();
    private readonly List<CourseFeedSub> _feedSubs = new();

    private FeedbackAggregate() { }

    public static FeedbackAggregate Create(
        long courseId, string userId, DateTime reviewDate,
        string generalRemarks, long requestNum)
    {
        var aggregate = new FeedbackAggregate();
        aggregate.FeedMain = CourseFeedMain.Create(courseId, userId, reviewDate, generalRemarks, requestNum);
        return aggregate;
    }

    public void AddFeedbackItem(
        long reqSrl, long srlNum, long typeCode,
        long? typeNum, string? typeDesc)
    {
        var sub = CourseFeedSub.Create(
            FeedMain.FdReqNum, reqSrl, srlNum, typeCode, typeNum, typeDesc);
        _feedSubs.Add(sub);
    }

    public void UpdateRemarks(string remarks) => FeedMain.Update(remarks);
}
