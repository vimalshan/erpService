namespace TransactionService.Domain.ValueObjects;

/// <summary>
/// Journal Voucher type: INV-Invoice, CRD-Credit, PJV-Payment JV, JV-Journal Voucher
/// </summary>
public sealed record JournalVoucherType
{
    public string Value { get; }

    private JournalVoucherType(string value) => Value = value;

    public static JournalVoucherType Invoice => new("INV");
    public static JournalVoucherType Credit => new("CRD");
    public static JournalVoucherType PaymentJV => new("PJV");
    public static JournalVoucherType JournalVoucher => new("JV");

    public static JournalVoucherType From(string value) =>
        value?.Trim().ToUpperInvariant() switch
        {
            "INV" => Invoice,
            "CRD" => Credit,
            "PJV" => PaymentJV,
            "JV" => JournalVoucher,
            _ => throw new ArgumentException($"Invalid JV type: '{value}'")
        };

    public static implicit operator string(JournalVoucherType type) => type.Value;
    public override string ToString() => Value;
}
