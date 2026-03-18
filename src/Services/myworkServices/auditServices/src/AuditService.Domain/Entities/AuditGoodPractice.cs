using AuditService.Domain.Common;
using AuditService.Domain.Events;

namespace AuditService.Domain.Entities;

/// <summary>
/// AUDIT_GOODPRACTICE - Good practice (Aggregate Root).
/// </summary>
public sealed class AuditGoodPractice : AggregateRoot
{
    private readonly List<AuditGoodPracticeRating> _ratings = new();

    private AuditGoodPractice() { }

    public long PracticeId { get; private set; }
    public string PracticeTitle { get; private set; } = string.Empty;
    public string PracticeDescription { get; private set; } = string.Empty;
    public string PracticeBenefits { get; private set; } = string.Empty;
    public string PracticeRemarks { get; private set; } = string.Empty;
    public long PracticeProcess { get; private set; }
    public long PracticeEmpSysId { get; private set; }
    public long PracticeUnit { get; private set; }
    public long PracticeLastModifiedBy { get; private set; }
    public DateTime PracticeLastModifiedOn { get; private set; }
    public string? PracticeAttachment1 { get; private set; }
    public string? PracticeAttachment2 { get; private set; }

    public IReadOnlyCollection<AuditGoodPracticeRating> Ratings => _ratings.AsReadOnly();
    public double AverageRating => _ratings.Count == 0 ? 0 : _ratings.Average(r => r.PracticeRating);

    public static AuditGoodPractice Create(
        long practiceId, string title, string description, string benefits,
        string remarks, long process, long empSysId, long unit, long createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var practice = new AuditGoodPractice
        {
            PracticeId = practiceId,
            PracticeTitle = title,
            PracticeDescription = description,
            PracticeBenefits = benefits,
            PracticeRemarks = remarks,
            PracticeProcess = process,
            PracticeEmpSysId = empSysId,
            PracticeUnit = unit,
            PracticeLastModifiedBy = createdBy,
            PracticeLastModifiedOn = DateTime.UtcNow
        };

        practice.AddDomainEvent(new GoodPracticeCreatedEvent(practice.PracticeId, practice.PracticeTitle));
        return practice;
    }

    public void AddRating(long ratingId, long ratedBy, int rating)
    {
        if (rating < 1 || rating > 5) throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be 1–5.");

        var existingRating = _ratings.FirstOrDefault(r => r.PracticeRatingBy == ratedBy);
        if (existingRating != null)
            _ratings.Remove(existingRating);

        _ratings.Add(AuditGoodPracticeRating.Create(ratingId, PracticeId, ratedBy, rating));
        PracticeLastModifiedOn = DateTime.UtcNow;
    }

    public void Update(string title, string description, string benefits, string remarks, long modifiedBy)
    {
        PracticeTitle = title;
        PracticeDescription = description;
        PracticeBenefits = benefits;
        PracticeRemarks = remarks;
        PracticeLastModifiedBy = modifiedBy;
        PracticeLastModifiedOn = DateTime.UtcNow;
    }

    public void AttachFiles(string? attachment1, string? attachment2)
    {
        PracticeAttachment1 = attachment1;
        PracticeAttachment2 = attachment2;
    }
}
