namespace CommunityService.Domain.Interfaces;

public interface IEntity
{
    long Id { get; }
}

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    string EventType { get; }
}
