namespace DevelopmentService.Domain.Events;

public sealed record LearningPlanApprovedEvent(
    long ReqNum,
    char AppStatus) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
