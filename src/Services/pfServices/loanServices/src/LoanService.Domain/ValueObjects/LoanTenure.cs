namespace LoanService.Domain.ValueObjects;

public record LoanTenure
{
    public string Value { get; }

    public LoanTenure(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 10)
            throw new ArgumentException("Loan tenure must be non-empty and max 10 characters.", nameof(value));
        Value = value;
    }
}
