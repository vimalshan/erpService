namespace ReportingService.Domain.Events;

/// <summary>
/// Event fired when an appraisal goal is added
/// </summary>
public class AppraisalGoalAddedEvent : DomainEvent
{
    public string? GoalDescription { get; }
    public decimal? Weightage { get; }

    public AppraisalGoalAddedEvent(long aggregateId, string? goalDescription, decimal? weightage)
        : base(aggregateId, DateTime.UtcNow)
    {
        GoalDescription = goalDescription;
        Weightage = weightage;
    }
}
