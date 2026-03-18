namespace MedicineManagement.Domain.ValueObjects;

public sealed record MedicineCode
{
    public string Value { get; }

    public MedicineCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Medicine code must be 1-3 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(MedicineCode code) => code.Value;
    public static explicit operator MedicineCode(string value) => new(value);
    public override string ToString() => Value;
}
