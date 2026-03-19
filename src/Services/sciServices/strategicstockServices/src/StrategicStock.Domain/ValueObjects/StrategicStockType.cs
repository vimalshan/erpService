namespace StrategicStock.Domain.ValueObjects;

using Common;

public sealed class StrategicStockType : ValueObject
{
    public string Code { get; }

    private StrategicStockType(string code) => Code = code;

    public static readonly StrategicStockType Normal = new("NR");
    public static readonly StrategicStockType Emergency = new("EM");
    public static readonly StrategicStockType Buffer = new("BF");

    private static readonly Dictionary<string, StrategicStockType> ValidTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["NR"] = Normal,
        ["EM"] = Emergency,
        ["BF"] = Buffer
    };

    public static StrategicStockType FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Strategic stock type code is required.", nameof(code));

        if (!ValidTypes.TryGetValue(code, out var stockType))
            throw new ArgumentException($"Invalid strategic stock type code '{code}'. Valid codes: {string.Join(", ", ValidTypes.Keys)}.", nameof(code));

        return stockType;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Code;
}
