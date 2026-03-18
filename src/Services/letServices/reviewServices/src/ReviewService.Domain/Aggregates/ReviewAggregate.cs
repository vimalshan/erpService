using ReviewService.Domain.Common;
using ReviewService.Domain.Entities;
using ReviewService.Domain.Events;

namespace ReviewService.Domain.Aggregates;

/// <summary>
/// Aggregate root for the Review aggregate (REVIEW_MAIN + REVIEW_SUB).
/// </summary>
public class ReviewAggregate : AggregateRoot
{
    public ReviewMain ReviewMain { get; private set; } = null!;
    public IReadOnlyCollection<ReviewSub> ReviewSubs => _reviewSubs.AsReadOnly();
    private readonly List<ReviewSub> _reviewSubs = new();

    private ReviewAggregate() { }

    public static ReviewAggregate Create(
        long srlNum, long? fedNum, string? remarks1, string? remarks2,
        char status, DateTime? nextDate)
    {
        var aggregate = new ReviewAggregate();
        aggregate.ReviewMain = ReviewMain.Create(srlNum, fedNum, remarks1, remarks2, status, nextDate);
        return aggregate;
    }

    public void AddReviewDetail(
        long revNum, DateTime reviewDate, long reviewedBy,
        string reviewStatus, string? remarks)
    {
        var sub = ReviewSub.Create(ReviewMain.RevSrlNum, revNum, reviewDate, reviewedBy, reviewStatus, remarks);
        _reviewSubs.Add(sub);
        AddDomainEvent(new ReviewDetailAddedEvent(ReviewMain.RevSrlNum, revNum, reviewedBy));
    }

    public void ChangeStatus(char newStatus) => ReviewMain.UpdateStatus(newStatus);
}
