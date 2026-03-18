using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Events;

public record DealBatchCreatedEvent(long BatchId, DateTime DealDate, long DerType) : IDomainEvent;

public record DealBatchRejectedEvent(long BatchId, string Reason) : IDomainEvent;

public record DealCreatedEvent(long DealId, long BatchId, decimal? Amount, DateTime? MaturityDate) : IDomainEvent;

public record DealApprovedEvent(long DealId, long BatchId, long AppBusiness) : IDomainEvent;

public record DealRejectedEvent(long DealId, long BatchId, string Remarks) : IDomainEvent;

public record DealSettledEvent(long DealId, long SettlementId, decimal GainLossAmt) : IDomainEvent;
