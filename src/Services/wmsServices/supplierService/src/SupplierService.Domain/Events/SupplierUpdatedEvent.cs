using SupplierService.Domain.Common;

namespace SupplierService.Domain.Events;

public class SupplierUpdatedEvent : IDomainEvent
{
    public Entities.Supplier Supplier { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public SupplierUpdatedEvent(Entities.Supplier supplier)
    {
        Supplier = supplier;
    }
}
