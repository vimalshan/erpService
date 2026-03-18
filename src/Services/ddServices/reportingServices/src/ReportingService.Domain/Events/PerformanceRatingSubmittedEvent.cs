namespace ReportingService.Domain.Events;

/// <summary>
/// Event fired when a performance rating is submitted
/// </summary>
public class PerformanceRatingSubmittedEvent : DomainEvent
{
    public decimal? Rating { get; }
    public string? PerformanceCategory { get; }

    public PerformanceRatingSubmittedEvent(long aggregateId, decimal? rating, string? performanceCategory)
        : base(aggregateId, DateTime.UtcNow)
    {
        Rating = rating;
        PerformanceCategory = performanceCategory;
    }
}
