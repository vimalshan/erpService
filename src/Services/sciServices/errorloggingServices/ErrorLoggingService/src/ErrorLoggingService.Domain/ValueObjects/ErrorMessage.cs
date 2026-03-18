namespace ErrorLoggingService.Domain.ValueObjects;

public sealed class ErrorMessage
{
    public string Value { get; }

    private ErrorMessage(string value) => Value = value;

    public static ErrorMessage Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Error message cannot be empty.", nameof(value));
        if (value.Length > 4000)
            throw new ArgumentException("Error message cannot exceed 4000 characters.", nameof(value));
        return new ErrorMessage(value);
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ErrorMessage other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
