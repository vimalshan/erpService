namespace AuthorizationService.Domain.ValueObjects;

/// <summary>
/// RightCode value object
/// </summary>
public class RightCode
{
    public decimal Value { get; set; }

    public RightCode(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Right code must be non-negative", nameof(value));
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is RightCode other)
            return Value == other.Value;
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}
