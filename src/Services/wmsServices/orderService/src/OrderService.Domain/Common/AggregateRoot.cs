namespace OrderService.Domain.Common;

public abstract class AggregateRoot : Entity
{
    public int Id { get; protected set; }
}
