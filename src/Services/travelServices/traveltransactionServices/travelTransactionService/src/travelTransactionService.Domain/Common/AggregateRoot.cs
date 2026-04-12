namespace travelTransactionService.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    public int Version { get; protected set; }
}
