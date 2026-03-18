using MediatR;

namespace CashManagement.Domain.Events;

public record CashUnitCreatedEvent(long CashUnitId, string Name) : INotification;
public record CashReceiptRecordedEvent(long CashUnitId, decimal Amount, string? RefNo) : INotification;
public record CashDisbursementRecordedEvent(long CashUnitId, decimal Amount, string? RefNo) : INotification;
public record BankAccountCreatedEvent(long BankAccountId, string BankName, string AccountNo) : INotification;
public record BankTransactionRecordedEvent(long BankAccountId, string TxnType, decimal Amount) : INotification;
public record ChequeIssuedEvent(long BankAccountId, string ChequeNumber, string PayeeName, decimal Amount) : INotification;
public record ChequeBouncedEvent(long BankAccountId, string ChequeNumber, string Reason, decimal Amount) : INotification;
public record BankReconciledEvent(long BankAccountId, DateOnly ReconciliationDate, decimal Difference) : INotification;
