using AccountingService.Application.Common.Exceptions;
using AccountingService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Features.Transactions.Commands.CancelTransaction;

public class CancelTransactionCommandHandler : IRequestHandler<CancelTransactionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public CancelTransactionCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<bool> Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TransactionDetails
            .FirstOrDefaultAsync(t => t.TdTrustCode == request.TrustCode
                                   && t.TransactionId == request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TransactionDetail),
                $"{request.TrustCode}/{request.TransactionId}");

        entity.Cancel(request.CancelledBy);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
