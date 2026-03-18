using AttendanceService.Domain.Exceptions;

namespace AttendanceService.Domain.ValueObjects;

/// <summary>L = LOP, A = Adjustment</summary>
public sealed record LopType
{
    public static readonly LopType Lop = new("L");
    public static readonly LopType Adjustment = new("A");

    public string Value { get; }

    private LopType(string value) => Value = value;

    public static LopType From(string value) => value switch
    {
        "L" => Lop,
        "A" => Adjustment,
        _ => throw new DomainException($"Invalid LOP type '{value}'.")
    };

    public override string ToString() => Value;
}
