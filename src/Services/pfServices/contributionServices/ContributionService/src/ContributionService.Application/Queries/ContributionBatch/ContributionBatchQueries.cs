using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Exceptions;
using ContributionService.Domain.Interfaces;
using MediatR;

namespace ContributionService.Application.Queries.ContributionBatch;

public record GetContributionBatchByIdQuery(long BatchNo) : IRequest<ContributionMainDto>;
public record GetAllContributionBatchesQuery : IRequest<IReadOnlyList<ContributionMainDto>>;
public record GetContributionBatchesByStatusQuery(string Status) : IRequest<IReadOnlyList<ContributionMainDto>>;
public record GetContributionBatchesByDateRangeQuery(DateTime Start, DateTime End) : IRequest<IReadOnlyList<ContributionMainDto>>;

public class GetContributionBatchByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetContributionBatchByIdQuery, ContributionMainDto>
{
    public async Task<ContributionMainDto> Handle(GetContributionBatchByIdQuery request, CancellationToken ct)
    {
        var batch = await uow.ContributionMain.GetByIdAsync(request.BatchNo, ct)
            ?? throw new ContributionNotFoundException("ContributionBatch", request.BatchNo);
        return mapper.Map<ContributionMainDto>(batch);
    }
}

public class GetAllContributionBatchesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllContributionBatchesQuery, IReadOnlyList<ContributionMainDto>>
{
    public async Task<IReadOnlyList<ContributionMainDto>> Handle(GetAllContributionBatchesQuery request, CancellationToken ct)
    {
        var batches = await uow.ContributionMain.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ContributionMainDto>>(batches);
    }
}

public class GetContributionBatchesByStatusHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetContributionBatchesByStatusQuery, IReadOnlyList<ContributionMainDto>>
{
    public async Task<IReadOnlyList<ContributionMainDto>> Handle(GetContributionBatchesByStatusQuery request, CancellationToken ct)
    {
        var batches = await uow.ContributionMain.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<ContributionMainDto>>(batches);
    }
}

public class GetContributionBatchesByDateRangeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetContributionBatchesByDateRangeQuery, IReadOnlyList<ContributionMainDto>>
{
    public async Task<IReadOnlyList<ContributionMainDto>> Handle(GetContributionBatchesByDateRangeQuery request, CancellationToken ct)
    {
        var batches = await uow.ContributionMain.GetByDateRangeAsync(request.Start, request.End, ct);
        return mapper.Map<IReadOnlyList<ContributionMainDto>>(batches);
    }
}
