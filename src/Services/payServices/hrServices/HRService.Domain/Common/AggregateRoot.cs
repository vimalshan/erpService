namespace HRService.Domain.Common;

/// <summary>
/// Base class for aggregate roots
/// </summary>
public abstract class AggregateRoot : Entity
{
    public virtual int ConcurrencyStamp { get; set; }
}
