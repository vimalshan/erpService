using SupplierService.Domain.Common;

namespace SupplierService.Domain.Events;

public class SupplierActivatedEvent : IDomainEvent
{
    public int SupplierId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public SupplierActivatedEvent(int supplierId)
    {
        SupplierId = supplierId;
    }
}
