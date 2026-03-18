namespace MedicineManagement.Domain.ValueObjects;

public sealed record LotNumber
{
    public string Value { get; }

    public LotNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException("Lot number must be 1-50 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(LotNumber lot) => lot.Value;
    public static explicit operator LotNumber(string value) => new(value);
    public override string ToString() => Value;
}
