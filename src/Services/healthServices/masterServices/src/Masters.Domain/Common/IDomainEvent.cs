namespace Masters.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    string EventType { get; }
}
