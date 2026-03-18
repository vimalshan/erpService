using BankService.Domain.Common;

namespace BankService.Domain.Events;

public record BankCreatedEvent(string TrustCode, string BankCode) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record BankAccountCreatedEvent(string AccountNumber, string AccountTitle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ChequeIssuedEvent(long ChequeId, decimal Amount, string Payee) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ChequeClearedEvent(long ChequeId, DateTime ClearedDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ReconciliationCompletedEvent(long ReconId, long ChequeId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
