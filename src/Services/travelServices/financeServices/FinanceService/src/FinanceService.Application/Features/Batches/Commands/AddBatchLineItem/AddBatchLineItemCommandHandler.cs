using FinanceService.Application.Common.Exceptions;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Batches.Commands.AddBatchLineItem;

public class AddBatchLineItemCommandHandler : IRequestHandler<AddBatchLineItemCommand, bool>
{
    private readonly IFinanceDbContext _context;

    public AddBatchLineItemCommandHandler(IFinanceDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddBatchLineItemCommand request, CancellationToken cancellationToken)
    {
        var batch = await _context.TravelBatchMains
            .FirstOrDefaultAsync(b => b.UnitCode == request.UnitCode && b.BatchNumber == request.BatchNumber, cancellationToken)
            ?? throw new NotFoundException(nameof(TravelBatchMain), $"{request.UnitCode}-{request.BatchNumber}");

        var maxSerialNum = await _context.TravelBatchSubs
            .Where(s => s.UnitCode == request.UnitCode && s.BatchNumber == request.BatchNumber)
            .MaxAsync(s => (decimal?)s.SerialNumber, cancellationToken) ?? 0;

        var lineItem = new TravelBatchSub
        {
            UnitCode = request.UnitCode,
            BatchNumber = request.BatchNumber,
            SerialNumber = maxSerialNum + 1,
            BookingNumber = request.BookingNumber,
            TicketCost = request.TicketCost,
            Status = "N"
        };

        _context.TravelBatchSubs.Add(lineItem);

        batch.Total = (batch.Total ?? 0) + request.TicketCost + request.GstAmount;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
