namespace AlertsNotifications.Domain.ValueObjects;

public sealed record AlertType
{
    public static readonly AlertType WorkflowDirect = new("WD");
    public static readonly AlertType WorkflowOthers = new("WO");
    public static readonly AlertType ScheduleDirect = new("SD");
    public static readonly AlertType ScheduleOthers = new("SO");

    public string Value { get; }

    private AlertType(string value) => Value = value;

    public static AlertType From(string value)
    {
        return value switch
        {
            "WD" => WorkflowDirect,
            "WO" => WorkflowOthers,
            "SD" => ScheduleDirect,
            "SO" => ScheduleOthers,
            _ => throw new ArgumentException($"Invalid alert type: {value}", nameof(value))
        };
    }

    public override string ToString() => Value;
}
