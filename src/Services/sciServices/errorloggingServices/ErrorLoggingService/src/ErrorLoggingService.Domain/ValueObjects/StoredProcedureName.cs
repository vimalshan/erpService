namespace ErrorLoggingService.Domain.ValueObjects;

public sealed class StoredProcedureName
{
    public string Value { get; }

    private StoredProcedureName(string value) => Value = value;

    public static StoredProcedureName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stored procedure name cannot be empty.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Stored procedure name cannot exceed 100 characters.", nameof(value));
        return new StoredProcedureName(value);
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is StoredProcedureName other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
