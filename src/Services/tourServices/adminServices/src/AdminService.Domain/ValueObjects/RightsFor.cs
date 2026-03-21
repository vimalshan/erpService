namespace AdminService.Domain.ValueObjects;

public record RightsFor
{
    public string Value { get; }

    public static readonly RightsFor Admin = new("Admin");
    public static readonly RightsFor Finance = new("Finance");

    private static readonly HashSet<string> ValidValues = new() { "Admin", "Finance" };

    private RightsFor(string value) => Value = value;

    public static RightsFor From(string value)
    {
        if (!ValidValues.Contains(value))
            throw new ArgumentException($"Invalid rights-for value: {value}. Valid values: {string.Join(", ", ValidValues)}");
        return new RightsFor(value);
    }

    public override string ToString() => Value;
}
