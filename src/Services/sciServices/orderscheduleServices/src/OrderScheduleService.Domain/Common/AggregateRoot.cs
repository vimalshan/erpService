namespace OrderScheduleService.Domain.Common;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot() { }
    protected AggregateRoot(long id)
    {
        Id = id;
    }
}
