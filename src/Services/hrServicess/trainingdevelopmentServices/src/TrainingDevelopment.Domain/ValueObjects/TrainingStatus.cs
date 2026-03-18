namespace TrainingDevelopment.Domain.ValueObjects;

public sealed class TrainingStatus
{
    public static readonly TrainingStatus Pending = new("P");
    public static readonly TrainingStatus Completed = new("C");
    public static readonly TrainingStatus Dropped = new("D");

    public string Value { get; }

    private TrainingStatus(string value) => Value = value;

    public static TrainingStatus From(string value) => value switch
    {
        "P" => Pending,
        "C" => Completed,
        "D" => Dropped,
        _ => throw new ArgumentException($"Invalid training status: {value}", nameof(value))
    };

    public static implicit operator string(TrainingStatus status) => status.Value;
    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is TrainingStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
