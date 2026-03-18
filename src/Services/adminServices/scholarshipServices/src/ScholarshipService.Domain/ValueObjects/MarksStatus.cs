namespace ScholarshipService.Domain.ValueObjects;

/// <summary>Marks status of a scholarship detail record: S=Scheduled, P=Uploaded/Pending, A=Approved, R=Rejected</summary>
public sealed class MarksStatus
{
    public static readonly MarksStatus Scheduled = new("S");
    public static readonly MarksStatus Pending = new("P");
    public static readonly MarksStatus Approved = new("A");
    public static readonly MarksStatus Rejected = new("R");

    public string Value { get; }

    private MarksStatus(string value) => Value = value;

    public static MarksStatus From(string value) => value switch
    {
        "S" => Scheduled,
        "P" => Pending,
        "A" => Approved,
        "R" => Rejected,
        _ => throw new ArgumentException($"Invalid MarksStatus: {value}", nameof(value))
    };

    public override string ToString() => Value;
    public static implicit operator string(MarksStatus s) => s.Value;
    public override bool Equals(object? obj) => obj is MarksStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
