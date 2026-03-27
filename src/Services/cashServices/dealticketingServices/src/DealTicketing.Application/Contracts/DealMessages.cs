namespace DealTicketing.Application.Contracts;

public record DealBatchCreatedMessage(long BatchId, DateTime DealDate, long DerType, DateTime OccurredAt);
public record DealApprovedMessage(long DealId, long BatchId, long AppBusiness, DateTime OccurredAt);
public record DealRejectedMessage(long DealId, long BatchId, string Remarks, DateTime OccurredAt);
public record DealSettledMessage(long DealId, long SettlementId, decimal GainLossAmt, DateTime OccurredAt);
