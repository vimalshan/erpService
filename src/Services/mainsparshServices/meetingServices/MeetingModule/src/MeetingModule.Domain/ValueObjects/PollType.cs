namespace MeetingModule.Domain.ValueObjects;

public sealed record PollType
{
    public static readonly PollType MultipleChoice = new("MULTIPLE_CHOICE");
    public static readonly PollType YesNo = new("YES_NO");
    public static readonly PollType Rating = new("RATING");
    public static readonly PollType Text = new("TEXT");

    public string Value { get; }

    private PollType(string value) => Value = value;

    private static readonly PollType[] All = [MultipleChoice, YesNo, Rating, Text];

    public static PollType From(string value) =>
        All.FirstOrDefault(t => t.Value == value)
        ?? throw new ArgumentException($"Invalid poll type: {value}");

    public override string ToString() => Value;
}
