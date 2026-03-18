namespace TimesheetService.Domain.ValueObjects;

public sealed class ApprovalStatus : IEquatable<ApprovalStatus>
{
    public static readonly ApprovalStatus Pending  = new("PENDING");
    public static readonly ApprovalStatus Approved = new("APPROVED");
    public static readonly ApprovalStatus Rejected = new("REJECTED");

    public string Value { get; }

    private ApprovalStatus(string value) => Value = value;

    public static ApprovalStatus From(string value) =>
        value?.ToUpperInvariant() switch
        {
            "PENDING"  => Pending,
            "APPROVED" => Approved,
            "REJECTED" => Rejected,
            _          => throw new ArgumentException($"Invalid ApprovalStatus: {value}")
        };

    public bool Equals(ApprovalStatus? other) => Value == other?.Value;
    public override bool Equals(object? obj) => obj is ApprovalStatus s && Equals(s);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(ApprovalStatus status) => status.Value;
}
