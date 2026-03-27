using MediatR;
using TransactionProcessing.Application.DTOs;

namespace TransactionProcessing.Application.Queries;

public sealed record GetTransactionByIdQuery(long TxnId) : IRequest<FinancialTransactionDto?>;

public sealed record GetTransactionsByBatchQuery(long BatchId) : IRequest<IReadOnlyList<FinancialTransactionDto>>;

public sealed record GetSettlementsByDealQuery(long DealId) : IRequest<IReadOnlyList<DealSettlementDto>>;

public sealed record GetDisbursementsByLoanQuery(long LoanId) : IRequest<IReadOnlyList<LoanDisbursementDto>>;

public sealed record GetRepaymentsByLoanQuery(long LoanId) : IRequest<IReadOnlyList<LoanRepaymentDto>>;

public sealed record GetTransactionLedgerQuery(DateTime? From, DateTime? To, string? Status, string? TxnType, int PageSize = 50, int Page = 1)
    : IRequest<IReadOnlyList<TransactionLedgerDto>>;
