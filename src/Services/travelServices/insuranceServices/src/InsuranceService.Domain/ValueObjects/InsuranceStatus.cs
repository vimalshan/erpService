namespace InsuranceService.Domain.ValueObjects;

public record InsuranceStatus
{
    public static readonly InsuranceStatus Active = new("A");
    public static readonly InsuranceStatus Inactive = new("I");
    public static readonly InsuranceStatus Expired = new("E");

    public string Value { get; }

    public InsuranceStatus(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 1)
            throw new ArgumentException("Insurance status must be a single character.", nameof(value));

        Value = value.Trim().ToUpperInvariant();

        if (Value is not ("A" or "I" or "E"))
            throw new ArgumentException("Insurance status must be A (Active), I (Inactive), or E (Expired).", nameof(value));
    }

    public static implicit operator string(InsuranceStatus status) => status.Value;
    public static explicit operator InsuranceStatus(string value) => new(value);
}
