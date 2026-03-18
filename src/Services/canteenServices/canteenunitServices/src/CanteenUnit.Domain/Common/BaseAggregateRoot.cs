namespace CanteenUnit.Domain.Common;

public abstract class BaseAggregateRoot : BaseEntity
{
    public int Version { get; protected set; }
}
