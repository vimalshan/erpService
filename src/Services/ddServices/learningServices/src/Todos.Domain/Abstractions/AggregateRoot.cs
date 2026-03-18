namespace Todos.Domain.Abstractions;

/// <summary>
/// Represents an aggregate root
/// </summary>
public abstract class AggregateRoot : Entity
{
    /// <summary>
    /// Gets the version of this aggregate
    /// </summary>
    public int Version { get; protected set; } = 0;

    /// <summary>
    /// Gets the date when this aggregate was created
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Gets the date when this aggregate was last modified
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }
}
