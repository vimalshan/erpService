namespace RackingSystem.Domain.ValueObjects;

public sealed record RackType
{
    public static readonly RackType Pallet = new("PALLET");
    public static readonly RackType Cantilever = new("CANTILEVER");
    public static readonly RackType DriveIn = new("DRIVE-IN");
    public static readonly RackType Shelving = new("SHELVING");
    public static readonly RackType Mobile = new("MOBILE");

    private static readonly HashSet<string> ValidTypes =
    [
        Pallet.Value, Cantilever.Value, DriveIn.Value, Shelving.Value, Mobile.Value
    ];

    public string Value { get; }

    private RackType(string value) => Value = value;

    public static RackType From(string value)
    {
        var normalised = value?.ToUpperInvariant() ?? string.Empty;
        if (!ValidTypes.Contains(normalised))
            throw new ArgumentException($"'{value}' is not a valid RackType.", nameof(value));
        return new RackType(normalised);
    }

    public static implicit operator string(RackType rt) => rt.Value;
    public override string ToString() => Value;
}
