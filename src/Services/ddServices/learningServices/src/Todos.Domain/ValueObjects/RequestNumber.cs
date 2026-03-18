using Todos.Domain.Abstractions;

namespace Todos.Domain.ValueObjects;

/// <summary>
/// Value object for request/learning & training record number
/// </summary>
public class RequestNumber : ValueObject
{
    /// <summary>
    /// Gets the request number
    /// </summary>
    public decimal Value { get; }

    public RequestNumber(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Request number must be greater than zero", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
