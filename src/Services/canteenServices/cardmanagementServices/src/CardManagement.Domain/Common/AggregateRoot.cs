namespace CardManagement.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    public long Id { get; protected set; }
}
