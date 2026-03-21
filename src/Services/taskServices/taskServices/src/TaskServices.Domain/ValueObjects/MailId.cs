using TaskServices.Domain.Common;

namespace TaskServices.Domain.ValueObjects;

public class MailId : ValueObject
{
    public decimal Value { get; }

    public MailId(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("MailId must be a positive number.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator decimal(MailId mailId) => mailId.Value;
    public static explicit operator MailId(decimal value) => new(value);
}
