namespace CompensationService.Domain.Common;

/// <summary>
/// Base class for aggregate roots
/// </summary>
public abstract class AggregateRoot : Entity
{
    public int Version { get; set; }

    protected AggregateRoot()
    {
        Version = 1;
    }
}
