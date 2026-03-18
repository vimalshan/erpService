using BusServices.Domain.Common;
using BusServices.Domain.Exceptions;

namespace BusServices.Domain.ValueObjects;

public sealed class RouteStatus : ValueObject
{
    public static readonly RouteStatus Active   = new('A');
    public static readonly RouteStatus Inactive = new('I');
    public static readonly RouteStatus Suspended = new('S');

    public char Value { get; }

    private RouteStatus(char value) => Value = value;

    public static RouteStatus Create(char value)
    {
        return value switch
        {
            'A' => Active,
            'I' => Inactive,
            'S' => Suspended,
            _ => throw new DomainException($"Invalid route status '{value}'. Valid: A, I, S.")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
