namespace LeaveServices.Domain.Common;

public abstract class AggregateRoot : Entity
{
    public long Id { get; protected set; }
}
