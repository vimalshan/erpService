namespace ActionService.Domain.Entities;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
