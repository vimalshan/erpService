using LetTransactionService.Domain.Common;
using LetTransactionService.Domain.Events;
using LetTransactionService.Domain.Exceptions;

namespace LetTransactionService.Domain.Entities;

/// <summary>Maps to COURSE_FEEDBACKMAIN table — course feedback header.</summary>
public class CourseFeedbackMain : BaseEntity
{
    public long FeedbackNumber { get; private set; }
    public long NominationNumber { get; private set; }
    public char? StatusCode { get; private set; }
    public DateTime? FeedbackDate { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public long? OverallRating { get; private set; }
    public string? Remarks1 { get; private set; }
    public string? Remarks2 { get; private set; }
    public string? Remarks3 { get; private set; }
    public decimal? FeedbackReviewSerial { get; private set; }
    public string? CancelRemark { get; private set; }
    public long? RequestNumber { get; private set; }
    public string? Remarks9 { get; private set; }
    public string? Remarks4 { get; private set; }
    public string? Remarks5 { get; private set; }
    public long? TotalManHours { get; private set; }
    public string? Remarks7 { get; private set; }
    public string? Remarks8 { get; private set; }

    private readonly List<CourseFeedbackSub> _feedbackDetails = [];
    public IReadOnlyList<CourseFeedbackSub> FeedbackDetails => _feedbackDetails.AsReadOnly();

    private CourseFeedbackMain() { }

    public static CourseFeedbackMain Create(
        long feedbackNumber,
        long nominationNumber,
        long? requestNumber,
        long? overallRating,
        string? remarks1,
        string? remarks2,
        string? remarks3,
        long? totalManHours)
    {
        var entity = new CourseFeedbackMain
        {
            FeedbackNumber = feedbackNumber,
            NominationNumber = nominationNumber,
            StatusCode = 'Y',
            FeedbackDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            OverallRating = overallRating,
            Remarks1 = remarks1,
            Remarks2 = remarks2,
            Remarks3 = remarks3,
            RequestNumber = requestNumber,
            TotalManHours = totalManHours
        };

        entity.AddDomainEvent(new FeedbackSubmittedEvent(feedbackNumber, nominationNumber));
        return entity;
    }

    public CourseFeedbackSub AddDetail(long feedbackType, long rating, string? remarks)
    {
        var detail = CourseFeedbackSub.Create(FeedbackNumber, feedbackType, rating, remarks);
        _feedbackDetails.Add(detail);
        return detail;
    }

    public void Cancel(string cancelRemark)
    {
        if (StatusCode == 'N')
            throw new LetDomainException("Feedback is already cancelled.");

        StatusCode = 'N';
        CancelRemark = cancelRemark;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new FeedbackCancelledEvent(FeedbackNumber, cancelRemark));
    }

    public void UpdateRating(long overallRating)
    {
        OverallRating = overallRating;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new FeedbackRatingUpdatedEvent(FeedbackNumber, overallRating));
    }
}
