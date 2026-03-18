using BusServices.Domain.Common;
using BusServices.Domain.Exceptions;

namespace BusServices.Domain.ValueObjects;

public sealed class ArrivalStatus : ValueObject
{
    public static readonly ArrivalStatus OnTime  = new('O');
    public static readonly ArrivalStatus Late    = new('L');
    public static readonly ArrivalStatus Absent  = new('A');
    public static readonly ArrivalStatus Early   = new('E');

    public char Value { get; }

    private ArrivalStatus(char value) => Value = value;

    public static ArrivalStatus Create(char value)
    {
        return value switch
        {
            'O' => OnTime,
            'L' => Late,
            'A' => Absent,
            'E' => Early,
            _ => throw new DomainException($"Invalid arrival status '{value}'. Valid: O, L, A, E.")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
