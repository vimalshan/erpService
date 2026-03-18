namespace LeaveServices.Domain.ValueObjects;

public sealed class EncashmentStatus : IEquatable<EncashmentStatus>
{
    public static readonly EncashmentStatus Pending = new('P', "Pending");
    public static readonly EncashmentStatus Approved = new('A', "Approved");
    public static readonly EncashmentStatus Rejected = new('R', "Rejected");
    public static readonly EncashmentStatus Processed = new('D', "Processed");

    private static readonly IReadOnlyList<EncashmentStatus> _all = [Pending, Approved, Rejected, Processed];

    public char Code { get; }
    public string Description { get; }

    private EncashmentStatus(char code, string description)
    {
        Code = code;
        Description = description;
    }

    public static EncashmentStatus From(char code)
    {
        var match = _all.FirstOrDefault(s => s.Code == code);
        if (match is null)
            throw new ArgumentException($"'{code}' is not a valid encashment status.", nameof(code));
        return match;
    }

    public override string ToString() => Description;
    public bool Equals(EncashmentStatus? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is EncashmentStatus es && Equals(es);
    public override int GetHashCode() => Code.GetHashCode();
}
