using PurchaseSalesService.Domain.Common;

namespace PurchaseSalesService.Domain.ValueObjects;

public sealed class SupplierCode : ValueObject
{
    public string Value { get; }

    private SupplierCode(string value) => Value = value;

    public static SupplierCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Supplier code cannot be empty.", nameof(value));
        if (value.Length > 25)
            throw new ArgumentException("Supplier code cannot exceed 25 characters.", nameof(value));
        return new SupplierCode(value.Trim().ToUpperInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
