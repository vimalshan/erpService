namespace LetTransactionService.Domain.Entities;

/// <summary>Maps to COURSE_FEEDBACKSUB table — feedback parameter details.</summary>
public class CourseFeedbackSub
{
    public long FeedbackNumber { get; private set; }
    public long FeedbackType { get; private set; }
    public long Rating { get; private set; }
    public string? Remarks { get; private set; }

    // Navigation
    public CourseFeedbackMain FeedbackMain { get; private set; } = null!;

    private CourseFeedbackSub() { }

    internal static CourseFeedbackSub Create(long feedbackNumber, long feedbackType, long rating, string? remarks)
    {
        return new CourseFeedbackSub
        {
            FeedbackNumber = feedbackNumber,
            FeedbackType = feedbackType,
            Rating = rating,
            Remarks = remarks
        };
    }
}
