namespace WorkOrderService.Domain.ValueObjects;

public sealed class WorkTaskStatus : IEquatable<WorkTaskStatus>
{
    public static readonly WorkTaskStatus Open = new('O', "Open");
    public static readonly WorkTaskStatus Completed = new('C', "Completed");
    public static readonly WorkTaskStatus Archived = new('A', "Archived");
    public static readonly WorkTaskStatus Paused = new('P', "Paused");

    public char Code { get; }
    public string Name { get; }

    private WorkTaskStatus(char code, string name)
    {
        Code = code;
        Name = name;
    }

    public static WorkTaskStatus FromCode(char code) => code switch
    {
        'O' => Open,
        'C' => Completed,
        'A' => Archived,
        'P' => Paused,
        _ => throw new ArgumentException($"Invalid task status code: {code}")
    };

    public static IEnumerable<WorkTaskStatus> GetAll() => [Open, Completed, Archived, Paused];

    public bool Equals(WorkTaskStatus? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is WorkTaskStatus status && Equals(status);
    public override int GetHashCode() => Code.GetHashCode();
    public override string ToString() => Name;

    public static bool operator ==(WorkTaskStatus? left, WorkTaskStatus? right) =>
        left is null ? right is null : left.Equals(right);
    public static bool operator !=(WorkTaskStatus? left, WorkTaskStatus? right) => !(left == right);
}
