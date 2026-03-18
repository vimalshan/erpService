namespace AlertsNotifications.Domain.ValueObjects;

public sealed record GradeCategory
{
    public static readonly GradeCategory All = new("ALL");
    public static readonly GradeCategory NonManagementStaff = new("NMS");
    public static readonly GradeCategory Officer = new("OFF");

    public string Value { get; }

    private GradeCategory(string value) => Value = value;

    public static GradeCategory From(string? value)
    {
        if (value is null) return All;
        return value switch
        {
            "ALL" => All,
            "NMS" => NonManagementStaff,
            "OFF" => Officer,
            _ => throw new ArgumentException($"Invalid grade category: {value}", nameof(value))
        };
    }

    public override string ToString() => Value;
}
