using LookupService.Domain.Common;

namespace LookupService.Domain.ValueObjects;

public class AccessType : ValueObject
{
    public string Value { get; }

    private AccessType(string value)
    {
        Value = value;
    }

    public static AccessType Create(string type)
    {
        if (string.IsNullOrWhiteSpace(type) || type.Length > 2)
            throw new ArgumentException("Access type must be 1-2 characters.", nameof(type));

        return new AccessType(type.PadRight(2));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.Trim();
}
