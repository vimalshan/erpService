namespace TransactionService.Domain.ValueObjects;

/// <summary>
/// Posting status for Journal Vouchers: P-Pending, Y-Posted, R-Reversed, C-Cancelled
/// </summary>
public sealed record PostingStatus
{
    public string Value { get; }

    private PostingStatus(string value) => Value = value;

    public static PostingStatus Pending => new("P");
    public static PostingStatus Posted => new("Y");
    public static PostingStatus Reversed => new("R");
    public static PostingStatus Cancelled => new("C");

    public static PostingStatus From(string value) =>
        value switch
        {
            "P" => Pending,
            "Y" => Posted,
            "R" => Reversed,
            "C" => Cancelled,
            _ => throw new ArgumentException($"Invalid posting status: '{value}'")
        };

    public static implicit operator string(PostingStatus status) => status.Value;
    public override string ToString() => Value;
}
