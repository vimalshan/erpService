namespace BankService.Infrastructure.Messaging;

public record ChequeIssuedMessage
{
    public long ChequeId { get; init; }
    public decimal Amount { get; init; }
    public string Payee { get; init; } = null!;
    public DateTime IssuedDate { get; init; }
}

public record ChequeClearedMessage
{
    public long ChequeId { get; init; }
    public DateTime ClearedDate { get; init; }
}

public record ReconciliationRequestedMessage
{
    public long ChequeId { get; init; }
    public string ReconReference { get; init; } = null!;
    public decimal ReconAmount { get; init; }
}
