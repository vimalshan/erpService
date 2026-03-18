namespace ItemMasterService.Domain.ValueObjects;

public sealed class ItemCode : IEquatable<ItemCode>
{
    public long Value { get; }

    private ItemCode(long value) => Value = value;

    public static ItemCode Create(long value)
    {
        if (value <= 0) throw new ArgumentException("Item code must be positive.", nameof(value));
        return new ItemCode(value);
    }

    public bool Equals(ItemCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is ItemCode ic && Equals(ic);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static implicit operator long(ItemCode code) => code.Value;
}
