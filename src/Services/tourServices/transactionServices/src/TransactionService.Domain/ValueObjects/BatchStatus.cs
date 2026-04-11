namespace TransactionService.Domain.ValueObjects;

/// <summary>
/// Batch status: P-Pending, C-Cancelled, Y-Admin Approved, A-Finance Approved, J-JV Posted, R-Rejected
/// </summary>
public sealed record BatchStatus
{
    public string Value { get; }

    private BatchStatus(string value) => Value = value;

    public static BatchStatus Pending => new("P");
    public static BatchStatus Cancelled => new("C");
    public static BatchStatus AdminApproved => new("Y");
    public static BatchStatus FinanceApproved => new("A");
    public static BatchStatus JVPosted => new("J");
    public static BatchStatus Rejected => new("R");

    public static BatchStatus From(string value) =>
        value?.Trim().ToUpperInvariant() switch
        {
            "P" => Pending,
            "C" => Cancelled,
            "Y" => AdminApproved,
            "A" => FinanceApproved,
            "J" => JVPosted,
            "R" => Rejected,
            _ => throw new ArgumentException($"Invalid batch status: '{value}'")
        };

    public static implicit operator string(BatchStatus status) => status.Value;
    public override string ToString() => Value;
}
