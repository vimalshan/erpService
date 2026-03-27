using MediatR;

namespace TransactionProcessing.Domain.Events;

public sealed record TransactionRecordedEvent(long TxnId, string TxnType, decimal Amount, string SourceService) : INotification;
public sealed record SettlementProcessedEvent(long SettlementId, long DealId, long SetId, char SettlementType, decimal NetAmount) : INotification;
public sealed record DisbursementProcessedEvent(long DisbProcId, long LoanId, long DisbId, decimal Amount) : INotification;
public sealed record RepaymentProcessedEvent(long RepayProcId, long LoanId, long RepayId, decimal Amount) : INotification;
public sealed record BatchCompletedEvent(long BatchId, string BatchType, int SuccessCount, int FailureCount, decimal TotalAmount) : INotification;
