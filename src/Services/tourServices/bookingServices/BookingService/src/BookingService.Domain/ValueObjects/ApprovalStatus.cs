namespace BookingService.Domain.ValueObjects;

public sealed record ApprovalStatus
{
    public static readonly ApprovalStatus Pending = new("PENDING");
    public static readonly ApprovalStatus Approved = new("APPROVED");
    public static readonly ApprovalStatus Rejected = new("REJECTED");

    public string Value { get; }

    private ApprovalStatus(string value) => Value = value;

    public static ApprovalStatus From(string value) => value.ToUpperInvariant() switch
    {
        "PENDING" => Pending,
        "APPROVED" => Approved,
        "REJECTED" => Rejected,
        _ => throw new ArgumentException($"Invalid approval status: {value}")
    };

    public override string ToString() => Value;
}
