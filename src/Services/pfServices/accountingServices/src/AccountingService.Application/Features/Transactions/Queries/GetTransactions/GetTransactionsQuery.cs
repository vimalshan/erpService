using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery(string TrustCode) : IRequest<IEnumerable<TransactionDetailDto>>;
