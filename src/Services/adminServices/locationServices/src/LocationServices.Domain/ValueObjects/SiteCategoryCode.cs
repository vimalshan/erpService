using LocationServices.Domain.Common;

namespace LocationServices.Domain.ValueObjects;

/// <summary>Value object representing site category code</summary>
public sealed class SiteCategoryCode : ValueObject
{
    public long? Value { get; }

    private SiteCategoryCode(long? value) => Value = value;

    public static SiteCategoryCode None => new(null);
    public static SiteCategoryCode Create(long value)
    {
        if (value < 0)
            throw new ArgumentException("SiteCategoryCode cannot be negative.", nameof(value));
        return new SiteCategoryCode(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value ?? (object)"NULL";
    }

    public override string ToString() => Value?.ToString() ?? "N/A";
}
