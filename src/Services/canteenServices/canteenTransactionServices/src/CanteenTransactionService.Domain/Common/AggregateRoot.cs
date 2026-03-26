namespace CanteenTransactionService.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    private int _version = 0;
    public int Version => _version;

    protected void IncrementVersion() => _version++;
}
