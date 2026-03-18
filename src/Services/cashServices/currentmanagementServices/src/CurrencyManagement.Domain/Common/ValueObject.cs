namespace CurrencyManagement.Domain.Common;

/// <summary>
/// Base class for value objects - immutable objects identified by their values, not by identity
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the atomic values that comprise the value object
    /// </summary>
    protected abstract IEnumerable<object?> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(HashCode), (hashCode, value) =>
            {
                hashCode.Add(value);
                return hashCode;
            })
            .ToHashCode();
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (Equals(left, null) && Equals(right, null))
            return true;

        if (Equals(left, null) || Equals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
