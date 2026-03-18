namespace Stationery.Domain.ValueObjects;

public sealed record RequestStatus
{
    public static readonly RequestStatus Pending  = new("P");
    public static readonly RequestStatus Approved = new("A");
    public static readonly RequestStatus Received = new("R");
    public static readonly RequestStatus Cancelled = new("X");
    public static readonly RequestStatus Closed   = new("C");
    public static readonly RequestStatus Indented  = new("I");

    public string Value { get; }

    private RequestStatus(string value) => Value = value;

    public static RequestStatus From(string value) => value switch
    {
        "P" => Pending,
        "A" => Approved,
        "R" => Received,
        "X" => Cancelled,
        "C" => Closed,
        "I" => Indented,
        _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unknown status: {value}")
    };

    public bool IsTerminal => Value is "C" or "X" or "R";

    public override string ToString() => Value;

    public static implicit operator string(RequestStatus status) => status.Value;
}
