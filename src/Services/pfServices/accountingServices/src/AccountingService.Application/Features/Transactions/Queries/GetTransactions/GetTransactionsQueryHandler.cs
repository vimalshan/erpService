using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Features.Transactions.Queries.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<TransactionDetailDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TransactionDetails
            .Where(t => t.TdTrustCode == request.TrustCode)
            .Select(t => new TransactionDetailDto(t.TdTrustCode, t.TransactionId, t.TdTransactionCode,
                t.TdTransactionType, t.TdTransactionDate, t.TdAmount, t.TdRemarks,
                t.TdMemberNo, t.TdTypeCode, t.TdFinyear, t.TdJvVoucherType, t.TdJvNo,
                t.TdCancelStatus != null))
            .ToListAsync(cancellationToken);
    }
}
