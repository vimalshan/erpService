namespace TransactionService.Domain.ValueObjects;

/// <summary>
/// Transaction type: ADV-Advance, EXP-Expense, ADJ-Adjustment, SET-Settlement, PER-Personal
/// </summary>
public sealed record TransactionType
{
    public string Value { get; }

    private TransactionType(string value) => Value = value;

    public static TransactionType Advance => new("ADV");
    public static TransactionType Expense => new("EXP");
    public static TransactionType Adjustment => new("ADJ");
    public static TransactionType Settlement => new("SET");
    public static TransactionType Personal => new("PER");

    public static TransactionType From(string value) =>
        value?.Trim().ToUpperInvariant() switch
        {
            "ADV" => Advance,
            "EXP" => Expense,
            "ADJ" => Adjustment,
            "SET" => Settlement,
            "PER" => Personal,
            _ => throw new ArgumentException($"Invalid transaction type: '{value}'")
        };

    public static implicit operator string(TransactionType type) => type.Value;
    public override string ToString() => Value;
}
