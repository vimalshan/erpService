namespace BankService.Domain.ValueObjects;

public record MicrCode
{
    public string Value { get; }

    public MicrCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 9)
            throw new ArgumentException("MICR code must be non-empty and at most 9 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(MicrCode code) => code.Value;
    public override string ToString() => Value;
}
