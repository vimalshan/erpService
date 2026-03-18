namespace DevelopmentService.Domain.Events;

public sealed record LearningPlanCreatedEvent(
    long ReqNum,
    string UserId,
    string? DevNeed) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
