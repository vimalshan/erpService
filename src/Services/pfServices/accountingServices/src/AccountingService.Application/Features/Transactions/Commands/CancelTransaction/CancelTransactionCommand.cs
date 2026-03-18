using MediatR;

namespace AccountingService.Application.Features.Transactions.Commands.CancelTransaction;

public record CancelTransactionCommand(string TrustCode, int TransactionId, string CancelledBy) : IRequest<bool>;
