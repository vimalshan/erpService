namespace LoanAccount.Domain.Common;

/// <summary>
/// Base class for all value objects in the domain
/// </summary>
public abstract class ValueObject
{
    public static bool operator ==(ValueObject? left, ValueObject? right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

    public override bool Equals(object? obj) => obj is ValueObject other && EqualsCore(other);
    protected abstract bool EqualsCore(ValueObject other);
    public abstract override int GetHashCode();
}
