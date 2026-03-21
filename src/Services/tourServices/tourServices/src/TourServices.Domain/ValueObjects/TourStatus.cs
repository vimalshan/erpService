namespace TourServices.Domain.ValueObjects;

public sealed class TourStatus : IEquatable<TourStatus>
{
    public static readonly TourStatus Planning   = new("P");
    public static readonly TourStatus Active     = new("A");
    public static readonly TourStatus Completed  = new("C");
    public static readonly TourStatus Cancelled  = new("X");

    public string Code { get; }

    private TourStatus(string code) => Code = code;

    public static TourStatus From(string code) => code?.Trim().ToUpperInvariant() switch
    {
        "P" or "PLANNING" or "PLANNED" => Planning,
        "A" or "ACTIVE"               => Active,
        "C" or "COMPLETED"            => Completed,
        "X" or "CANCELLED" or "CANCELED" => Cancelled,
        _ => throw new ArgumentException($"Invalid tour status: '{code}'. Use P/Planning, A/Active, C/Completed, X/Cancelled.", nameof(code))
    };

    public override string ToString() => Code;
    public bool Equals(TourStatus? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is TourStatus ts && Equals(ts);
    public override int GetHashCode() => Code.GetHashCode();
    public static implicit operator string(TourStatus ts) => ts.Code;
}
