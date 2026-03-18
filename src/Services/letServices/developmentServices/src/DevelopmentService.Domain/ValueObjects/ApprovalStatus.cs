namespace DevelopmentService.Domain.ValueObjects;

public sealed class ApprovalStatus : IEquatable<ApprovalStatus>
{
    public static readonly ApprovalStatus Pending      = new('F');
    public static readonly ApprovalStatus Approved     = new('A');
    public static readonly ApprovalStatus Rejected     = new('R');
    public static readonly ApprovalStatus BhrApproved  = new('B');

    public char Value { get; }

    private ApprovalStatus(char value) => Value = value;

    public static ApprovalStatus From(char value) => value switch
    {
        'F' => Pending,
        'A' => Approved,
        'R' => Rejected,
        'B' => BhrApproved,
        _   => throw new ArgumentException($"Invalid approval status: {value}")
    };

    public bool Equals(ApprovalStatus? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is ApprovalStatus s && Equals(s);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
