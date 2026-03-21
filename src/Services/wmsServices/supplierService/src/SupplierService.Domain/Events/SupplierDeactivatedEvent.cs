using SupplierService.Domain.Common;

namespace SupplierService.Domain.Events;

public class SupplierDeactivatedEvent : IDomainEvent
{
    public int SupplierId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public SupplierDeactivatedEvent(int supplierId)
    {
        SupplierId = supplierId;
    }
}
