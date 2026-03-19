namespace MobileAppManagement.Domain.ValueObjects;

public record DeviceId
{
    public string Value { get; }

    public DeviceId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Device ID cannot be empty.", nameof(value));
        if (value.Length > 200)
            throw new ArgumentException("Device ID cannot exceed 200 characters.", nameof(value));
        Value = value;
    }

    public static implicit operator string(DeviceId deviceId) => deviceId.Value;
    public static explicit operator DeviceId(string value) => new(value);
}
