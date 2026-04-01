namespace LetTransactionService.Domain.Entities;

/// <summary>Maps to REVIEW_SUB table — individual review entries.</summary>
public class ReviewSub
{
    public long ReviewMainSerial { get; private set; }
    public long ReviewNumber { get; private set; }
    public char? NextRequired { get; private set; }
    public DateTime? ReviewDate { get; private set; }
    public long ReviewBy { get; private set; }
    public string? Remarks { get; private set; }
    public string? ReviewStatus { get; private set; }
    public string? ProgressRemarks { get; private set; }

    // Navigation
    public ReviewMain ReviewMain { get; private set; } = null!;

    private ReviewSub() { }

    internal static ReviewSub Create(
        long reviewMainSerial,
        long reviewNumber,
        char? nextRequired,
        DateTime? reviewDate,
        long reviewBy,
        string? remarks,
        string? progressRemarks)
    {
        return new ReviewSub
        {
            ReviewMainSerial = reviewMainSerial,
            ReviewNumber = reviewNumber,
            NextRequired = nextRequired,
            ReviewDate = reviewDate ?? DateTime.UtcNow,
            ReviewBy = reviewBy,
            Remarks = remarks,
            ReviewStatus = "N",
            ProgressRemarks = progressRemarks
        };
    }

    internal void MarkCompleted()
    {
        ReviewStatus = "Y";
    }
}
