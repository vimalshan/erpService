namespace EmployeeRelations.Domain.ValueObjects;

/// <summary>EWS flag value object: R=Red, G=Green, A=Amber.</summary>
public sealed class EwsFlag
{
    public static readonly EwsFlag Red = new("R");
    public static readonly EwsFlag Green = new("G");
    public static readonly EwsFlag Amber = new("A");

    public string Value { get; }

    private EwsFlag(string value) => Value = value;

    public static EwsFlag From(string value) => value switch
    {
        "R" => Red,
        "G" => Green,
        "A" => Amber,
        _ => throw new ArgumentException($"Invalid EWS flag: {value}")
    };

    public override string ToString() => Value;
}
