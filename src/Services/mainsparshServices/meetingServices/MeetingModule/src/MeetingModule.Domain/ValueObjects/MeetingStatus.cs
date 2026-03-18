namespace MeetingModule.Domain.ValueObjects;

public sealed record MeetingStatus
{
    public static readonly MeetingStatus Scheduled = new("SCHEDULED");
    public static readonly MeetingStatus Ongoing = new("ONGOING");
    public static readonly MeetingStatus Completed = new("COMPLETED");
    public static readonly MeetingStatus Cancelled = new("CANCELLED");

    public string Value { get; }

    private MeetingStatus(string value) => Value = value;

    private static readonly MeetingStatus[] All = [Scheduled, Ongoing, Completed, Cancelled];

    public static MeetingStatus From(string value) =>
        All.FirstOrDefault(s => s.Value == value)
        ?? throw new ArgumentException($"Invalid meeting status: {value}");

    public override string ToString() => Value;
}
