namespace AlertsNotifications.Domain.ValueObjects;

public sealed record AlertGroupType
{
    public static readonly AlertGroupType ReportingUnit = new("R");
    public static readonly AlertGroupType PayrollUnit = new("P");
    public static readonly AlertGroupType CalendarWise = new("C");

    public string Value { get; }

    private AlertGroupType(string value) => Value = value;

    public static AlertGroupType From(string value)
    {
        return value switch
        {
            "R" => ReportingUnit,
            "P" => PayrollUnit,
            "C" => CalendarWise,
            _ => throw new ArgumentException($"Invalid alert group type: {value}", nameof(value))
        };
    }

    public override string ToString() => Value;
}
