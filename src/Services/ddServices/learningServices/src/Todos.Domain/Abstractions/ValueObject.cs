namespace Todos.Domain.Abstractions;

/// <summary>
/// Base class for all value objects
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the equality components for comparison
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        var valueObject = (ValueObject)obj;
        return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
    }

    /// <summary>
    /// Serves as the default hash function
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Determines whether two specified instances are equal
    /// </summary>
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances are not equal
    /// </summary>
    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !left.Equals(right);
    }
}
