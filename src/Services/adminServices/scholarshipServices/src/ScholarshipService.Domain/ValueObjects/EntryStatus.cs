namespace ScholarshipService.Domain.ValueObjects;

/// <summary>Entry status of a scholarship application: E=Entered, A=Approved, N=Not Eligible, B=Returned</summary>
public sealed class EntryStatus
{
    public static readonly EntryStatus Entered = new("E");
    public static readonly EntryStatus Approved = new("A");
    public static readonly EntryStatus NotEligible = new("N");
    public static readonly EntryStatus Returned = new("B");

    public string Value { get; }

    private EntryStatus(string value) => Value = value;

    public static EntryStatus From(string value) => value switch
    {
        "E" => Entered,
        "A" => Approved,
        "N" => NotEligible,
        "B" => Returned,
        _ => throw new ArgumentException($"Invalid EntryStatus: {value}", nameof(value))
    };

    public override string ToString() => Value;
    public static implicit operator string(EntryStatus s) => s.Value;
    public override bool Equals(object? obj) => obj is EntryStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
