using DispatchPlanning.Domain.Common;

namespace DispatchPlanning.Domain.ValueObjects;

public sealed record PlanType
{
    public static readonly PlanType Itemwise = new('I');
    public static readonly PlanType SubGroupwise = new('S');

    public char Value { get; }

    private PlanType(char value) => Value = value;

    public static PlanType From(char value) => value switch
    {
        'I' => Itemwise,
        'S' => SubGroupwise,
        _ => throw new ArgumentException($"Invalid plan type '{value}'. Allowed: I, S.")
    };

    public override string ToString() => Value.ToString();
}
