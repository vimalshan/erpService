namespace HRService.Domain.Entities;

public enum PerformanceReviewStatus
{
    Draft,
    Submitted,
    Approved
}

public class PerformanceReview : Common.AggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime ReviewPeriodStart { get; private set; }
    public DateTime ReviewPeriodEnd { get; private set; }
    public decimal Rating { get; private set; } // 1-5 scale
    public string? Comments { get; private set; }
    public Guid ReviewedBy { get; private set; }
    public PerformanceReviewStatus Status { get; private set; } = PerformanceReviewStatus.Draft;
    public DateTime? ReviewDate { get; private set; }

    private PerformanceReview() { }

    public static PerformanceReview Create(
        Guid employeeId,
        DateTime reviewPeriodStart,
        DateTime reviewPeriodEnd,
        Guid reviewedBy)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee id cannot be empty", nameof(employeeId));

        if (reviewedBy == Guid.Empty)
            throw new ArgumentException("Reviewed by id cannot be empty", nameof(reviewedBy));

        if (reviewPeriodEnd < reviewPeriodStart)
            throw new ArgumentException("Review period end must be after start");

        return new PerformanceReview
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            ReviewPeriodStart = reviewPeriodStart,
            ReviewPeriodEnd = reviewPeriodEnd,
            ReviewedBy = reviewedBy,
            Status = PerformanceReviewStatus.Draft,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void SetRating(decimal rating, string? comments = null)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

        Rating = rating;
        Comments = comments;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Submit()
    {
        if (Status != PerformanceReviewStatus.Draft)
            throw new InvalidOperationException("Only draft reviews can be submitted");

        Status = PerformanceReviewStatus.Submitted;
        ReviewDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;

        var @event = new Events.PerformanceReviewSubmittedEvent
        {
            ReviewId = Id,
            EmployeeId = EmployeeId,
            Rating = Rating
        };

        AddDomainEvent(@event);
    }

    public void Approve()
    {
        if (Status != PerformanceReviewStatus.Submitted)
            throw new InvalidOperationException("Only submitted reviews can be approved");

        Status = PerformanceReviewStatus.Approved;
        ModifiedDate = DateTime.UtcNow;
    }
}
