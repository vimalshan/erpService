using MediatR;

namespace InventoryManagement.Domain.Common;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
