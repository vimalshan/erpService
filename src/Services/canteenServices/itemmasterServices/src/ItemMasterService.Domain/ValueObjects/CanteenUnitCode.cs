using ItemMasterService.Domain.Common;

namespace ItemMasterService.Domain.ValueObjects;

public sealed class CanteenUnitCode : IEquatable<CanteenUnitCode>
{
    public long Value { get; }

    private CanteenUnitCode(long value) => Value = value;

    public static CanteenUnitCode Create(long value)
    {
        if (value <= 0) throw new ArgumentException("Canteen unit code must be positive.", nameof(value));
        return new CanteenUnitCode(value);
    }

    public bool Equals(CanteenUnitCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is CanteenUnitCode vo && Equals(vo);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static implicit operator long(CanteenUnitCode code) => code.Value;
}
