using PFTransactionalService.Domain.Common;

namespace PFTransactionalService.Domain.Events;

public record PFAccumulationCreatedEvent(long EmpSysId, long MemberNo, string TrustCode, decimal InitialBalance) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ContributionPostedEvent(long EmpSysId, long MemberNo, decimal EmpContribution, decimal ErContribution, DateTime TxnMonth) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record WithdrawalProcessedEvent(long EmpSysId, long MemberNo, decimal Amount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record InterestAppliedEvent(long EmpSysId, long MemberNo, decimal InterestAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record PFAccumulationClosedEvent(long EmpSysId, long MemberNo) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record PFSettlementCreatedEvent(long SettlementId, long EmpSysId, decimal Amount, string SettlementType) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record CertificateGeneratedEvent(long CertificateId, long SettlementId, long EmpSysId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
