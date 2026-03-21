namespace InsuranceService.Domain.ValueObjects;

public record CompanyCode
{
    public string Value { get; }

    public CompanyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Company code must be 1-3 characters.", nameof(value));

        Value = value.Trim();
    }

    public static implicit operator string(CompanyCode code) => code.Value;
    public static explicit operator CompanyCode(string value) => new(value);
}
