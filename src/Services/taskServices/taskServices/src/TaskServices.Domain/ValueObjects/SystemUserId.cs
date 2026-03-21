using TaskServices.Domain.Common;

namespace TaskServices.Domain.ValueObjects;

public class SystemUserId : ValueObject
{
    public decimal Value { get; }

    public SystemUserId(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("SystemUserId must be a positive number.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator decimal(SystemUserId sysId) => sysId.Value;
    public static explicit operator SystemUserId(decimal value) => new(value);
}
