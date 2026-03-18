namespace SwipeTransactionService.Domain.ValueObjects;

public sealed record ItemCode
{
    public long Value { get; }

    public ItemCode(long value)
    {
        if (value <= 0) throw new ArgumentException("Item code must be positive.", nameof(value));
        Value = value;
    }

    public static implicit operator long(ItemCode code) => code.Value;
    public static implicit operator ItemCode(long value) => new(value);
    public override string ToString() => Value.ToString();
}
