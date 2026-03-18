using AuditService.Domain.Common;

namespace AuditService.Domain.Entities;

/// <summary>
/// AUDIT_GOODPRACTICERATING - Rating record for a good practice.
/// </summary>
public sealed class AuditGoodPracticeRating : BaseEntity
{
    private AuditGoodPracticeRating() { }

    public long PracticeRatingId { get; private set; }
    public long PracticeId { get; private set; }
    public long PracticeRatingBy { get; private set; }
    public int PracticeRating { get; private set; }
    public DateTime PracticeLastModifiedOn { get; private set; }

    internal static AuditGoodPracticeRating Create(long ratingId, long practiceId, long ratingBy, int rating)
    {
        return new AuditGoodPracticeRating
        {
            PracticeRatingId = ratingId,
            PracticeId = practiceId,
            PracticeRatingBy = ratingBy,
            PracticeRating = rating,
            PracticeLastModifiedOn = DateTime.UtcNow
        };
    }
}
