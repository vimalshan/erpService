using SupplierService.Domain.Common;

namespace SupplierService.Domain.Events;

public class SupplierCreatedEvent : IDomainEvent
{
    public Entities.Supplier Supplier { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public SupplierCreatedEvent(Entities.Supplier supplier)
    {
        Supplier = supplier;
    }
}
