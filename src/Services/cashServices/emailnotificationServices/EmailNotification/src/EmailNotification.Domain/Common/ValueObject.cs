namespace EmailNotification.Domain.Common;

/// <summary>
/// Base class for value objects
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the components of the value object for equality comparison
    /// </summary>
    /// <returns>Enumerable of objects representing the value object components</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object is equal to the current value object
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>true if the objects are equal; otherwise, false</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Returns the hash code for the value object
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Determines whether two specified value objects are equal
    /// </summary>
    /// <param name="left">The first value object to compare</param>
    /// <param name="right">The second value object to compare</param>
    /// <returns>true if the value objects are equal; otherwise, false</returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified value objects are not equal
    /// </summary>
    /// <param name="left">The first value object to compare</param>
    /// <param name="right">The second value object to compare</param>
    /// <returns>true if the value objects are not equal; otherwise, false</returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}
