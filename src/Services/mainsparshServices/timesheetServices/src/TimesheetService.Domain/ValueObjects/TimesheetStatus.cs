namespace TimesheetService.Domain.ValueObjects;

public sealed class TimesheetStatus : IEquatable<TimesheetStatus>
{
    public static readonly TimesheetStatus Draft     = new("DRAFT");
    public static readonly TimesheetStatus Submitted = new("SUBMITTED");
    public static readonly TimesheetStatus Approved  = new("APPROVED");
    public static readonly TimesheetStatus Rejected  = new("REJECTED");

    public string Value { get; }

    private TimesheetStatus(string value) => Value = value;

    public static TimesheetStatus From(string value) =>
        value?.ToUpperInvariant() switch
        {
            "DRAFT"     => Draft,
            "SUBMITTED" => Submitted,
            "APPROVED"  => Approved,
            "REJECTED"  => Rejected,
            _           => throw new ArgumentException($"Invalid TimesheetStatus: {value}")
        };

    public bool Equals(TimesheetStatus? other) => Value == other?.Value;
    public override bool Equals(object? obj) => obj is TimesheetStatus s && Equals(s);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(TimesheetStatus status) => status.Value;
}
