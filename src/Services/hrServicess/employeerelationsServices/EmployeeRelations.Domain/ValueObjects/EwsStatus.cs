namespace EmployeeRelations.Domain.ValueObjects;

/// <summary>EWS status value object.</summary>
public sealed class EwsStatus
{
    public static readonly EwsStatus PendingHr = new("N");
    public static readonly EwsStatus PendingAppraiser = new("A");
    public static readonly EwsStatus Completed = new("Y");

    public string Value { get; }

    private EwsStatus(string value) => Value = value;

    public static EwsStatus From(string value) => value switch
    {
        "N" => PendingHr,
        "A" => PendingAppraiser,
        "Y" => Completed,
        _ => throw new ArgumentException($"Invalid EWS status: {value}")
    };

    public override string ToString() => Value;
}
