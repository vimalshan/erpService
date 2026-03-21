using MediatR;

namespace WarehouseStructure.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
