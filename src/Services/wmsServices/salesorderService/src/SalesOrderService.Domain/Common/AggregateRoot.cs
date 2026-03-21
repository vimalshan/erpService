namespace SalesOrderService.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    public int Id { get; protected set; }
}
