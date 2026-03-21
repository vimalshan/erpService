namespace AuditLogService.Domain.ValueObjects;

public sealed class AuditAction
{
    public static readonly AuditAction Insert = new("INSERT");
    public static readonly AuditAction Update = new("UPDATE");
    public static readonly AuditAction Delete = new("DELETE");

    public string Value { get; }

    private AuditAction(string value) => Value = value;

    public static AuditAction From(string value)
    {
        return value.ToUpperInvariant() switch
        {
            "INSERT" => Insert,
            "UPDATE" => Update,
            "DELETE" => Delete,
            _ => throw new ArgumentException($"Invalid audit action: {value}")
        };
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is AuditAction other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
