namespace CashManagement.Application.DTOs;

public record CashUnitDto(
    long CashUnitId,
    string Name,
    string Code,
    string? Location,
    long? InChargeEmployeeId,
    decimal OpeningBalance,
    string Status,
    decimal CurrentBalance,
    DateTime CreatedOn
);

public record CashTransactionDto(
    long CashTxnId,
    long CashUnitId,
    string TxnType,
    decimal Amount,
    string? Source,
    long? PayeeId,
    string? RefNo,
    DateTime TxnDate,
    string? Remarks,
    string Status,
    long? AuthorizedBy,
    long CreatedBy,
    DateTime CreatedOn
);

public record BankAccountDto(
    long BankAccountId,
    string BankName,
    string AccountNo,
    string? Branch,
    string? AccountType,
    string Status,
    DateTime CreatedOn
);

public record BankTransactionDto(
    long BankTxnId,
    long BankAccountId,
    string TxnType,
    decimal Amount,
    DateTime TxnDate,
    string? Reference,
    string? Remarks,
    string Status,
    long CreatedBy,
    DateTime CreatedOn
);

public record ChequeDto(
    long ChequeId,
    long BankAccountId,
    string ChequeNumber,
    string PayeeName,
    decimal ChequeAmount,
    DateOnly IssueDate,
    DateOnly ChequeDate,
    string? Reference,
    string Status,
    string? BounceReason,
    DateTime CreatedOn
);

public record BankReconciliationDto(
    long ReconId,
    long BankAccountId,
    decimal BankStatementBalance,
    decimal LedgerBalance,
    decimal? UnclearedCheques,
    decimal? DifferenceAmount,
    string? Status,
    DateOnly ReconciliationDate,
    long CreatedBy,
    DateTime CreatedOn
);

public record CashBalanceDto(long CashUnitId, string UnitName, decimal Balance, DateTime AsOfDate);
public record BankBalanceDto(long BankAccountId, string BankName, string AccountNo, decimal Balance, DateTime AsOfDate);
