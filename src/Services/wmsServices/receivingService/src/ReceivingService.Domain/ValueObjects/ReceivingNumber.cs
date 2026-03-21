namespace ReceivingService.Domain.ValueObjects;

public sealed class ReceivingNumber
{
    public string Value { get; }

    private ReceivingNumber(string value) => Value = value;

    public static ReceivingNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Receiving number cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Receiving number cannot exceed 50 characters.", nameof(value));
        return new ReceivingNumber(value.Trim());
    }

    /// <summary>Generate a prefixed receiving number: RCV-yyyyMMdd-XXXX</summary>
    public static ReceivingNumber Generate() =>
        Create($"RCV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}");

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ReceivingNumber n && Value == n.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
