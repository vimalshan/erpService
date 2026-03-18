using ReviewService.Domain.Common;
using ReviewService.Domain.Events;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to REVIEW_MAIN table.
/// </summary>
public class ReviewMain : BaseEntity
{
    public long RevSrlNum { get; private set; }
    public long? RevFedNum { get; private set; }
    public string? RevRemMrk1 { get; private set; }
    public string? RevRemMrk2 { get; private set; }
    public string? RevRemMrk3 { get; private set; }
    public string? RevRemMrk4 { get; private set; }
    public string? RevRemMrk5 { get; private set; }
    public string? RevRemMrk6 { get; private set; }
    public string? RevRemMrk7 { get; private set; }
    public string? RevRemMrk8 { get; private set; }
    public string? RevRemMrk9 { get; private set; }
    public string? RevRemMrk10 { get; private set; }
    public string? RevEntDate { get; private set; }
    public char? RevStatus { get; private set; }
    public DateTime? RevNextDate { get; private set; }

    public ICollection<ReviewSub> ReviewSubs { get; private set; } = new List<ReviewSub>();

    private ReviewMain() { }

    public static ReviewMain Create(
        long srlNum,
        long? fedNum,
        string? remarks1,
        string? remarks2,
        char status,
        DateTime? nextDate)
    {
        var review = new ReviewMain
        {
            RevSrlNum = srlNum,
            RevFedNum = fedNum,
            RevRemMrk1 = remarks1,
            RevRemMrk2 = remarks2,
            RevStatus = status,
            RevEntDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            RevNextDate = nextDate
        };
        review.AddDomainEvent(new ReviewSubmittedEvent(srlNum, fedNum, status));
        return review;
    }

    public void UpdateStatus(char newStatus)
    {
        RevStatus = newStatus;
        AddDomainEvent(new ReviewStatusChangedEvent(RevSrlNum, newStatus));
    }

    public void SetNextDate(DateTime nextDate) => RevNextDate = nextDate;
}
