using FilingAndArchiveService.Domain.Common;

namespace FilingAndArchiveService.Domain.ValueObjects;

public sealed class OrganizationId : ValueObject
{
    public string Value { get; }

    private OrganizationId(string value) => Value = value;

    public static OrganizationId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Organization ID cannot be empty.", nameof(value));

        if (value.Length > 25)
            throw new ArgumentException("Organization ID cannot exceed 25 characters.", nameof(value));

        return new OrganizationId(value.Trim());
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
