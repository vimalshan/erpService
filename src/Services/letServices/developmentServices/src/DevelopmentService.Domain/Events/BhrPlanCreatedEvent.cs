namespace DevelopmentService.Domain.Events;

public sealed record BhrPlanCreatedEvent(
    long ReqNum,
    string UserId,
    string? TrainingProgram) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
