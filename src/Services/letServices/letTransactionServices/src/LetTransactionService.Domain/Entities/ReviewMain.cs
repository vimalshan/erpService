using LetTransactionService.Domain.Common;
using LetTransactionService.Domain.Events;
using LetTransactionService.Domain.Exceptions;

namespace LetTransactionService.Domain.Entities;

/// <summary>Maps to REVIEW_MAIN table — review form with implementation goals.</summary>
public class ReviewMain : BaseEntity
{
    public long ReviewSerialNumber { get; private set; }
    public long FeedbackNumber { get; private set; }
    public string? ImplementationGoal { get; private set; }
    public string? KeyLearning { get; private set; }
    public string? KeyStepsImplementation { get; private set; }
    public string? KeyOutputsExpected { get; private set; }
    public string? MeasurementProcess { get; private set; }
    public string? HelpRequiredFromHr { get; private set; }
    public string? AdditionalRemarks1 { get; private set; }
    public string? AdditionalRemarks2 { get; private set; }
    public string? AdditionalRemarks3 { get; private set; }
    public string? AdditionalRemarks4 { get; private set; }
    public string? EntryDate { get; private set; }
    public char? Status { get; private set; }
    public DateTime? NextReviewDate { get; private set; }

    private readonly List<ReviewSub> _reviewDetails = [];
    public IReadOnlyList<ReviewSub> ReviewDetails => _reviewDetails.AsReadOnly();

    private ReviewMain() { }

    public static ReviewMain Create(
        long reviewSerialNumber,
        long feedbackNumber,
        string? implementationGoal,
        string? keyLearning,
        string? keyStepsImplementation,
        string? keyOutputsExpected,
        string? measurementProcess,
        string? helpRequiredFromHr,
        DateTime? nextReviewDate)
    {
        var entity = new ReviewMain
        {
            ReviewSerialNumber = reviewSerialNumber,
            FeedbackNumber = feedbackNumber,
            ImplementationGoal = implementationGoal,
            KeyLearning = keyLearning,
            KeyStepsImplementation = keyStepsImplementation,
            KeyOutputsExpected = keyOutputsExpected,
            MeasurementProcess = measurementProcess,
            HelpRequiredFromHr = helpRequiredFromHr,
            EntryDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Status = 'N',
            NextReviewDate = nextReviewDate
        };

        entity.AddDomainEvent(new ReviewCreatedEvent(reviewSerialNumber, feedbackNumber));
        return entity;
    }

    public ReviewSub AddReviewDetail(
        long reviewNumber,
        char? nextRequired,
        DateTime? reviewDate,
        long reviewBy,
        string? remarks,
        string? progressRemarks)
    {
        var detail = ReviewSub.Create(
            ReviewSerialNumber, reviewNumber, nextRequired,
            reviewDate, reviewBy, remarks, progressRemarks);

        _reviewDetails.Add(detail);
        AddDomainEvent(new ReviewSubAddedEvent(ReviewSerialNumber, reviewNumber));
        return detail;
    }

    public void Approve()
    {
        if (Status == 'A')
            throw new LetDomainException("Review is already approved.");

        Status = 'A';
        AddDomainEvent(new ReviewApprovedEvent(ReviewSerialNumber));
    }

    public void Resend()
    {
        if (Status != 'N')
            throw new LetDomainException("Only submitted reviews can be resent for correction.");

        Status = 'R';
    }
}
