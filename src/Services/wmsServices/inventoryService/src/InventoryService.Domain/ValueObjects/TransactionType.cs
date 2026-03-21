namespace InventoryService.Domain.ValueObjects;

public sealed record TransactionType
{
    private static readonly HashSet<string> ValidTypes =
    [
        "RECEIPT", "SHIPMENT", "MOVE_OUT", "MOVE_IN", "ADJUSTMENT",
        "PICK", "PACK", "RETURN", "IN", "OUT"
    ];

    public static readonly TransactionType Receipt = new("RECEIPT");
    public static readonly TransactionType Shipment = new("SHIPMENT");
    public static readonly TransactionType MoveOut = new("MOVE_OUT");
    public static readonly TransactionType MoveIn = new("MOVE_IN");
    public static readonly TransactionType Adjustment = new("ADJUSTMENT");
    public static readonly TransactionType Pick = new("PICK");
    public static readonly TransactionType Pack = new("PACK");
    public static readonly TransactionType Return = new("RETURN");
    public static readonly TransactionType In = new("IN");
    public static readonly TransactionType Out = new("OUT");

    public string Value { get; }

    private TransactionType(string value)
    {
        if (!ValidTypes.Contains(value))
            throw new ArgumentException($"Invalid transaction type: {value}");
        Value = value;
    }

    public static TransactionType From(string value) => new(value.ToUpperInvariant());

    public override string ToString() => Value;
}
