namespace LocationServices.Domain.Common;

/// <summary>Base class for value objects (immutable, equality by value)</summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(0, HashCode.Combine);

    public static bool operator ==(ValueObject left, ValueObject right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueObject left, ValueObject right) =>
        !(left == right);
}
