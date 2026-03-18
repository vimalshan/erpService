namespace TrainingDevelopment.Domain.ValueObjects;

public sealed class TrainingMode
{
    // 1 = On-The-Job, 2 = Classroom
    public decimal Value { get; }

    private TrainingMode(decimal value) => Value = value;

    public static TrainingMode OnTheJob => new(1);
    public static TrainingMode Classroom => new(2);

    public static TrainingMode From(decimal value) => value switch
    {
        1 => OnTheJob,
        2 => Classroom,
        _ => throw new ArgumentException($"Invalid training mode: {value}", nameof(value))
    };

    public string DisplayName => Value switch
    {
        1 => "On-The-Job",
        2 => "Classroom",
        _ => "Unknown"
    };

    public static implicit operator decimal(TrainingMode mode) => mode.Value;
    public override string ToString() => DisplayName;
    public override bool Equals(object? obj) => obj is TrainingMode other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
