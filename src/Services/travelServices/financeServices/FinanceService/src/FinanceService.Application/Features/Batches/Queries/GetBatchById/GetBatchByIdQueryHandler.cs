using AutoMapper;
using FinanceService.Application.Common.Exceptions;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Batches.Queries.GetBatchById;

public class GetBatchByIdQueryHandler : IRequestHandler<GetBatchByIdQuery, BatchDto>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public GetBatchByIdQueryHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BatchDto> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await _context.TravelBatchMains
            .Include(b => b.BatchLines)
            .FirstOrDefaultAsync(b => b.UnitCode == request.UnitCode && b.BatchNumber == request.BatchNumber, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TravelBatchMain), $"{request.UnitCode}-{request.BatchNumber}");

        return _mapper.Map<BatchDto>(batch);
    }
}
