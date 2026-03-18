namespace FillingOperationService.Domain.ValueObjects;

public sealed class CapacityPerShift
{
    public int Value { get; }

    private CapacityPerShift(int value) => Value = value;

    public static CapacityPerShift Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Capacity per shift must be a positive number.", nameof(value));
        return new CapacityPerShift(value);
    }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj is CapacityPerShift other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
