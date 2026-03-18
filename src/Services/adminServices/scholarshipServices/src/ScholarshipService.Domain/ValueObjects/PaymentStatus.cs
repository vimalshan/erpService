namespace ScholarshipService.Domain.ValueObjects;

/// <summary>Payment status: S=Scheduled, A=HR Approved, P=Pending, C=Completed, O=Offline Paid, B=Backdated</summary>
public sealed class PaymentStatus
{
    public static readonly PaymentStatus Scheduled = new("S");
    public static readonly PaymentStatus HrApproved = new("A");
    public static readonly PaymentStatus Pending = new("P");
    public static readonly PaymentStatus Completed = new("C");
    public static readonly PaymentStatus OfflinePaid = new("O");
    public static readonly PaymentStatus Backdated = new("B");

    public string Value { get; }

    private PaymentStatus(string value) => Value = value;

    public static PaymentStatus From(string value) => value switch
    {
        "S" => Scheduled,
        "A" => HrApproved,
        "P" => Pending,
        "C" => Completed,
        "O" => OfflinePaid,
        "B" => Backdated,
        _ => throw new ArgumentException($"Invalid PaymentStatus: {value}", nameof(value))
    };

    public override string ToString() => Value;
    public static implicit operator string(PaymentStatus s) => s.Value;
    public override bool Equals(object? obj) => obj is PaymentStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
