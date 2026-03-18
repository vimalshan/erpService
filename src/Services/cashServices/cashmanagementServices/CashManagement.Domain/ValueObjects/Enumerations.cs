namespace CashManagement.Domain.ValueObjects;

public enum CashTransactionType { Receipt = 'R', Disbursement = 'D' }
public enum BankTransactionType { Deposit = 'D', Withdrawal = 'W' }
public enum ChequeStatus { Issued = 'I', Cleared = 'C', Bounced = 'B', Cancelled = 'X' }
public enum EntityStatus { Active = 'A', Inactive = 'I' }
public enum TransactionStatus { Posted = 'P', Hold = 'H' }
public enum ReconciliationStatus { Reconciled, Difference }
