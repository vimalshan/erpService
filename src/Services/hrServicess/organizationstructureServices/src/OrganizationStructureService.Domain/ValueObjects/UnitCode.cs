namespace OrganizationStructureService.Domain.ValueObjects;

public sealed record UnitCode
{
    public string Value { get; }

    private UnitCode(string value) => Value = value;

    public static UnitCode From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("UnitCode must be 1-3 characters.", nameof(value));
        return new UnitCode(value.ToUpperInvariant());
    }

    public override string ToString() => Value;
}
