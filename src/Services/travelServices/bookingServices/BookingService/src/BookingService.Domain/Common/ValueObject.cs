namespace BookingService.Domain.Common;

public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        return ((ValueObject)obj).GetEqualityComponents().SequenceEqual(GetEqualityComponents());
    }
    public override int GetHashCode()
        => GetEqualityComponents()
            .Where(x => x != null)
            .Aggregate(0, (hash, item) => HashCode.Combine(hash, item!.GetHashCode()));

    public static bool operator ==(ValueObject? left, ValueObject? right)
        => left is null ? right is null : left.Equals(right);
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}
