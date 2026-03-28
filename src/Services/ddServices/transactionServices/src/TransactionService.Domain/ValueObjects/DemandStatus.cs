namespace TransactionService.Domain.ValueObjects;

public class DemandStatus
{
    public char Value { get; }

    public static readonly DemandStatus Open = new('O');
    public static readonly DemandStatus Approved = new('A');
    public static readonly DemandStatus Rejected = new('R');
    public static readonly DemandStatus Completed = new('C');

    private static readonly HashSet<char> ValidStatuses = ['O', 'A', 'R', 'C'];

    public DemandStatus(char value)
    {
        if (!ValidStatuses.Contains(value))
            throw new ArgumentException($"Invalid demand status: {value}. Must be one of: O, A, R, C");
        Value = value;
    }

    public bool IsOpen => Value == 'O';
    public bool IsApproved => Value == 'A';
    public bool IsRejected => Value == 'R';
    public bool IsCompleted => Value == 'C';

    public override bool Equals(object? obj) => obj is DemandStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
