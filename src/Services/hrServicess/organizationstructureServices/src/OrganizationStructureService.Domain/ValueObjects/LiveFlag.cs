namespace OrganizationStructureService.Domain.ValueObjects;

public sealed record LiveFlag
{
    public string Value { get; }

    private LiveFlag(string value) => Value = value;

    public static LiveFlag Active => new("Y");
    public static LiveFlag Inactive => new("N");

    public static LiveFlag From(string value)
    {
        if (value != "Y" && value != "N")
            throw new ArgumentException("LiveFlag must be 'Y' or 'N'.", nameof(value));
        return new LiveFlag(value);
    }

    public bool IsActive => Value == "Y";

    public override string ToString() => Value;
}
