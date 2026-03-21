namespace PurchaseOrderService.Domain.Common;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;
}
