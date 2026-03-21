using AutoMapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Batches.Commands.CreateBatch;

public class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, BatchDto>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public CreateBatchCommandHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BatchDto> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var maxBatchNum = await _context.TravelBatchMains
            .Where(b => b.UnitCode == request.UnitCode)
            .MaxAsync(b => (decimal?)b.BatchNumber, cancellationToken) ?? 0;

        var batch = TravelBatchMain.Create(
            request.UnitCode,
            maxBatchNum + 1,
            request.AgencyCode,
            request.InvoiceNum,
            request.AdminRemarks,
            request.TotalAmount);

        _context.TravelBatchMains.Add(batch);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BatchDto>(batch);
    }
}
