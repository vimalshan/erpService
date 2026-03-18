namespace ExitManagement.Domain.ValueObjects;

/// <summary>
/// Represents the type of resignation/exit for an employee.
/// </summary>
public sealed class ResignationType
{
    public string Value { get; }
    public string Description { get; }

    private ResignationType(string value, string description)
    {
        Value = value;
        Description = description;
    }

    public static readonly ResignationType Voluntary = new("VOLUNTARY", "Voluntary Resignation");
    public static readonly ResignationType Termination = new("TERMINATION", "Termination");
    public static readonly ResignationType Retirement = new("RETIREMENT", "Retirement");
    public static readonly ResignationType Death = new("DEATH", "Death");
    public static readonly ResignationType Transfer = new("TRANSFER", "Transfer");
    public static readonly ResignationType Absconding = new("ABSCONDING", "Absconding");

    public static ResignationType FromValue(string value) => value.ToUpperInvariant() switch
    {
        "VOLUNTARY" => Voluntary,
        "TERMINATION" => Termination,
        "RETIREMENT" => Retirement,
        "DEATH" => Death,
        "TRANSFER" => Transfer,
        "ABSCONDING" => Absconding,
        _ => new ResignationType(value, value)
    };

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is ResignationType other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
