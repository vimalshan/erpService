using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    string TrustCode,
    int TransactionId,
    string TransactionCode,
    DateTime TransactionDate,
    decimal Amount,
    string TypeCode,
    string ModifiedBy,
    long FinYear,
    string JvVoucherType,
    string JvNo,
    string? TransactionType = null,
    string? Remarks = null,
    int? MemberNo = null,
    string? ReferenceType = null,
    string? ContributionRefNo = null,
    string? TrnSubType = null
) : IRequest<TransactionDetailDto>;
