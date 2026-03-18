namespace MeetingModule.Domain.ValueObjects;

public sealed record PollStatus
{
    public static readonly PollStatus Active = new("ACTIVE");
    public static readonly PollStatus Closed = new("CLOSED");
    public static readonly PollStatus Archived = new("ARCHIVED");

    public string Value { get; }

    private PollStatus(string value) => Value = value;

    private static readonly PollStatus[] All = [Active, Closed, Archived];

    public static PollStatus From(string value) =>
        All.FirstOrDefault(s => s.Value == value)
        ?? throw new ArgumentException($"Invalid poll status: {value}");

    public override string ToString() => Value;
}
