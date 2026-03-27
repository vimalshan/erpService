namespace TransactionProcessing.Application.DTOs;

public sealed record FinancialTransactionDto(
    long TxnId,
    long? TxnBatchId,
    string TxnType,
    string? TxnSubType,
    decimal TxnAmount,
    long? TxnCurrencyId,
    decimal? TxnExchangeRate,
    decimal? TxnBaseAmount,
    string? TxnReference,
    string TxnSourceService,
    long? TxnSourceId,
    string TxnStatus,
    string? TxnRemarks,
    long CreatedBy,
    DateTime CreatedOn);

public sealed record DealSettlementDto(
    long SettlementId,
    long TxnId,
    long DealId,
    long SetId,
    string SettlementType,
    decimal? SpotRate,
    decimal? ExchangeRate,
    decimal SettlementAmount,
    decimal? GainLossAmount,
    decimal? PremiumAmount,
    decimal? WindingFee,
    decimal NetAmount,
    long? BankAccountId,
    string ProcessingStatus,
    DateTime CreatedOn);

public sealed record LoanDisbursementDto(
    long DisbProcId,
    long TxnId,
    long LoanId,
    long DisbId,
    decimal DisbAmount,
    decimal? ExchangeRate,
    decimal? ConvertedAmount,
    long? BankAccountId,
    string ProcessingStatus,
    DateTime CreatedOn);

public sealed record LoanRepaymentDto(
    long RepayProcId,
    long TxnId,
    long LoanId,
    long RepayId,
    decimal RepayAmount,
    decimal? ExchangeRate,
    decimal? ConvertedAmount,
    long? BankAccountId,
    string ProcessingStatus,
    DateTime CreatedOn);

public sealed record TransactionBatchDto(
    long BatchId,
    string BatchType,
    DateTime BatchDate,
    string BatchStatus,
    int? BatchTotalCount,
    int? BatchSuccessCount,
    int? BatchFailureCount,
    decimal? BatchTotalAmount,
    long CreatedBy,
    DateTime CreatedOn,
    DateTime? CompletedOn);

public sealed record TransactionLedgerDto(
    long TxnId,
    string TxnType,
    string? TxnReference,
    decimal TxnAmount,
    long? TxnCurrencyId,
    decimal? TxnBaseAmount,
    string TxnStatus,
    string TxnSourceService,
    DateTime CreatedOn,
    List<TransactionAuditDto> AuditTrail);

public sealed record TransactionAuditDto(
    long AuditId,
    string PreviousStatus,
    string NewStatus,
    string AuditAction,
    string? AuditRemarks,
    long AuditBy,
    DateTime AuditOn);
