namespace BatchService.Domain.ValueObjects;

/// <summary>Strongly-typed value object representing the status of a batch.</summary>
public sealed class BatchStatus : IEquatable<BatchStatus>
{
    public static readonly BatchStatus Open   = new('O');
    public static readonly BatchStatus Closed = new('C');
    public static readonly BatchStatus Locked = new('L');

    public char Value { get; }

    private BatchStatus(char value) => Value = value;

    public static BatchStatus From(char value) => value switch
    {
        'O' => Open,
        'C' => Closed,
        'L' => Locked,
        _   => throw new ArgumentException($"Invalid batch status '{value}'.")
    };

    public override string ToString() => Value.ToString();

    public bool Equals(BatchStatus? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is BatchStatus bs && Equals(bs);
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(BatchStatus? left, BatchStatus? right) => Equals(left, right);
    public static bool operator !=(BatchStatus? left, BatchStatus? right) => !Equals(left, right);
}
