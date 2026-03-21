namespace TourServices.Domain.ValueObjects;

public sealed class RegistrationStatus : IEquatable<RegistrationStatus>
{
    public static readonly RegistrationStatus Active    = new("A");
    public static readonly RegistrationStatus Cancelled = new("C");
    public static readonly RegistrationStatus Waitlist  = new("W");

    public string Code { get; }

    private RegistrationStatus(string code) => Code = code;

    public static RegistrationStatus From(string code) => code switch
    {
        "A" => Active,
        "C" => Cancelled,
        "W" => Waitlist,
        _ => throw new ArgumentException($"Invalid registration status code: {code}", nameof(code))
    };

    public override string ToString() => Code;
    public bool Equals(RegistrationStatus? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is RegistrationStatus rs && Equals(rs);
    public override int GetHashCode() => Code.GetHashCode();
    public static implicit operator string(RegistrationStatus rs) => rs.Code;
}
