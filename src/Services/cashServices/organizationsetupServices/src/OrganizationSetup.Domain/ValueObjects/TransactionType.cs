namespace OrganizationSetup.Domain.ValueObjects;

/// <summary>PP_TRANTYPE: 'I' = Import, 'E' = Export.</summary>
public sealed class TransactionType
{
    public const string Import = "I";
    public const string Export = "E";

    public string Value { get; }

    private TransactionType(string value) => Value = value;

    public static TransactionType Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var upper = value.Trim().ToUpperInvariant();
        if (upper is not Import and not Export)
            throw new ArgumentException($"Transaction type must be '{Import}' (Import) or '{Export}' (Export).", nameof(value));
        return new TransactionType(upper);
    }

    public bool IsImport => Value == Import;
    public bool IsExport => Value == Export;

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is TransactionType other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}
