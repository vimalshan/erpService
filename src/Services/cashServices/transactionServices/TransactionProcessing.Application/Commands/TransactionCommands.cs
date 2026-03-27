using MediatR;
using TransactionProcessing.Application.DTOs;

namespace TransactionProcessing.Application.Commands;

public sealed record ProcessDealSettlementCommand(
    long? BatchId,
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
    long? CurrencyId,
    long? BankAccountId,
    string? Remarks,
    long CreatedBy) : IRequest<DealSettlementDto>;

public sealed record ProcessLoanDisbursementCommand(
    long? BatchId,
    long LoanId,
    long DisbId,
    decimal DisbAmount,
    decimal? ExchangeRate,
    long? CurrencyId,
    long? BankAccountId,
    string? Remarks,
    long CreatedBy) : IRequest<LoanDisbursementDto>;

public sealed record ProcessLoanRepaymentCommand(
    long? BatchId,
    long LoanId,
    long RepayId,
    decimal RepayAmount,
    decimal? ExchangeRate,
    long? CurrencyId,
    long? BankAccountId,
    string? Remarks,
    long CreatedBy) : IRequest<LoanRepaymentDto>;

public sealed record ProcessCashTransferCommand(
    long? BatchId,
    decimal Amount,
    long? CurrencyId,
    decimal? ExchangeRate,
    string? SubType,
    string SourceService,
    long? SourceId,
    string? Remarks,
    long CreatedBy) : IRequest<FinancialTransactionDto>;

public sealed record CreateTransactionBatchCommand(
    string BatchType,
    DateTime BatchDate,
    long CreatedBy) : IRequest<TransactionBatchDto>;

public sealed record CompleteTransactionBatchCommand(
    long BatchId,
    long CompletedBy) : IRequest<TransactionBatchDto>;
