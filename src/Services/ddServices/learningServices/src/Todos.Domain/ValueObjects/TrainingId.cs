using Todos.Domain.Abstractions;

namespace Todos.Domain.ValueObjects;

/// <summary>
/// Value object for learning training identifier
/// </summary>
public class TrainingId : ValueObject
{
    /// <summary>
    /// Gets the training identifier
    /// </summary>
    public decimal Value { get; }

    public TrainingId(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Training ID must be greater than zero", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
