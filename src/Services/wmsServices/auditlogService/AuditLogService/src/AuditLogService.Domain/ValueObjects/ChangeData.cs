namespace AuditLogService.Domain.ValueObjects;

public sealed class ChangeData
{
    public string? OldValues { get; }
    public string? NewValues { get; }

    public ChangeData(string? oldValues, string? newValues)
    {
        OldValues = oldValues;
        NewValues = newValues;
    }

    public override bool Equals(object? obj) =>
        obj is ChangeData other && OldValues == other.OldValues && NewValues == other.NewValues;

    public override int GetHashCode() => HashCode.Combine(OldValues, NewValues);
}
