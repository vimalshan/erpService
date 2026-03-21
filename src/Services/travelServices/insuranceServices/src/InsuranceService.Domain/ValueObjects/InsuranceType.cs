namespace InsuranceService.Domain.ValueObjects;

public record InsuranceType
{
    public string Value { get; }

    public InsuranceType(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Insurance type must be 1-3 characters.", nameof(value));

        Value = value.Trim();
    }

    public static implicit operator string(InsuranceType type) => type.Value;
    public static explicit operator InsuranceType(string value) => new(value);
}
