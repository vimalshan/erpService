using AutoMapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Batches.Queries.GetAllBatches;

public class GetAllBatchesQueryHandler : IRequestHandler<GetAllBatchesQuery, List<BatchDto>>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBatchesQueryHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BatchDto>> Handle(GetAllBatchesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.TravelBatchMains
            .Include(b => b.BatchLines)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.UnitCode))
            query = query.Where(b => b.UnitCode == request.UnitCode);

        var batches = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<BatchDto>>(batches);
    }
}
