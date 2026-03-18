namespace SettlementService.Domain.ValueObjects;

public record TrustCode
{
    public string Value { get; init; }

    public TrustCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Trust code must be 1-3 characters.", nameof(value));
        Value = value.Trim();
    }
}
