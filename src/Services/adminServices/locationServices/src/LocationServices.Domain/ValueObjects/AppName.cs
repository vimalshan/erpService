using LocationServices.Domain.Common;

namespace LocationServices.Domain.ValueObjects;

/// <summary>Value object representing a valid AppName (DDD)</summary>
public sealed class AppName : ValueObject
{
    public const int MaxLength = 255;
    public string Value { get; }

    private AppName(string value) => Value = value;

    public static AppName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AppName cannot be empty.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"AppName cannot exceed {MaxLength} characters.", nameof(value));
        return new AppName(value.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToUpperInvariant();
    }

    public override string ToString() => Value;
    public static implicit operator string(AppName name) => name.Value;
    public static explicit operator AppName(string value) => Create(value);
}
