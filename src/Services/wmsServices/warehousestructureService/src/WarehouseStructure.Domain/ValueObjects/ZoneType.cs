namespace WarehouseStructure.Domain.ValueObjects;

public sealed class ZoneType : IEquatable<ZoneType>
{
    public static readonly ZoneType Receiving = new("RECEIVING");
    public static readonly ZoneType Storage = new("STORAGE");
    public static readonly ZoneType Picking = new("PICKING");
    public static readonly ZoneType Shipping = new("SHIPPING");
    public static readonly ZoneType Returns = new("RETURNS");
    public static readonly ZoneType Packing = new("PACKING");

    private static readonly HashSet<string> ValidTypes = new()
    {
        "RECEIVING", "STORAGE", "PICKING", "SHIPPING", "RETURNS", "PACKING"
    };

    public string Value { get; }

    public ZoneType(string value)
    {
        if (!ValidTypes.Contains(value.ToUpperInvariant()))
            throw new ArgumentException($"Invalid zone type: {value}. Valid types: {string.Join(", ", ValidTypes)}");

        Value = value.ToUpperInvariant();
    }

    public bool Equals(ZoneType? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as ZoneType);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(ZoneType zoneType) => zoneType.Value;
    public static explicit operator ZoneType(string value) => new(value);
}
