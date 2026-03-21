using FinanceService.Application.Common.Exceptions;
using FinanceService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Batches.Commands.ApproveBatch;

public class ApproveBatchCommandHandler : IRequestHandler<ApproveBatchCommand, bool>
{
    private readonly IFinanceDbContext _context;

    public ApproveBatchCommandHandler(IFinanceDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ApproveBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _context.TravelBatchMains
            .Include(b => b.BatchLines)
            .FirstOrDefaultAsync(b => b.UnitCode == request.UnitCode && b.BatchNumber == request.BatchNumber, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TravelBatchMain), $"{request.UnitCode}-{request.BatchNumber}");

        batch.Approve(request.ApprovalRemarks);

        foreach (var line in batch.BatchLines)
        {
            line.Status = "Y";
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
