using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Exceptions;
using ContributionService.Domain.Interfaces;
using MediatR;

namespace ContributionService.Application.Queries.SuperannuationBatch;

public record GetSuperannuationBatchByIdQuery(long BatchNo) : IRequest<SuperannuationBatchDto>;
public record GetAllSuperannuationBatchesQuery : IRequest<IReadOnlyList<SuperannuationBatchDto>>;

public class GetSuperannuationBatchByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetSuperannuationBatchByIdQuery, SuperannuationBatchDto>
{
    public async Task<SuperannuationBatchDto> Handle(GetSuperannuationBatchByIdQuery request, CancellationToken ct)
    {
        var batch = await uow.SuperannuationBatches.GetByIdAsync(request.BatchNo, ct)
            ?? throw new ContributionNotFoundException("SuperannuationBatch", request.BatchNo);
        return mapper.Map<SuperannuationBatchDto>(batch);
    }
}

public class GetAllSuperannuationBatchesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllSuperannuationBatchesQuery, IReadOnlyList<SuperannuationBatchDto>>
{
    public async Task<IReadOnlyList<SuperannuationBatchDto>> Handle(GetAllSuperannuationBatchesQuery request, CancellationToken ct)
    {
        var batches = await uow.SuperannuationBatches.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<SuperannuationBatchDto>>(batches);
    }
}
