namespace LovService.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
