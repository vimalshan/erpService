namespace WorkOrderService.Domain.ValueObjects;

public sealed class WorkOrderStatus : IEquatable<WorkOrderStatus>
{
    public static readonly WorkOrderStatus Open = new('O', "Open");
    public static readonly WorkOrderStatus Closed = new('C', "Closed");
    public static readonly WorkOrderStatus Archived = new('A', "Archived");

    public char Code { get; }
    public string Name { get; }

    private WorkOrderStatus(char code, string name)
    {
        Code = code;
        Name = name;
    }

    public static WorkOrderStatus FromCode(char code) => code switch
    {
        'O' => Open,
        'C' => Closed,
        'A' => Archived,
        _ => throw new ArgumentException($"Invalid work order status code: {code}")
    };

    public static IEnumerable<WorkOrderStatus> GetAll() => [Open, Closed, Archived];

    public bool Equals(WorkOrderStatus? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is WorkOrderStatus status && Equals(status);
    public override int GetHashCode() => Code.GetHashCode();
    public override string ToString() => Name;

    public static bool operator ==(WorkOrderStatus? left, WorkOrderStatus? right) =>
        left is null ? right is null : left.Equals(right);
    public static bool operator !=(WorkOrderStatus? left, WorkOrderStatus? right) => !(left == right);
}
