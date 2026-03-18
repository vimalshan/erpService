namespace AuthProvider.Domain.Common;

/// <summary>Value Object base – equality by structural value, not reference.</summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        return ((ValueObject)obj).GetAtomicValues().SequenceEqual(GetAtomicValues());
    }

    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(0, (hash, value) =>
            HashCode.Combine(hash, value?.GetHashCode() ?? 0));

    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left?.Equals(right) ?? right is null;
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}
