using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Interfaces;
using MediatR;

namespace ContributionService.Application.Queries.ContributionDetail;

public record GetContributionDetailsByBatchQuery(decimal BatchNo) : IRequest<IReadOnlyList<ContributionDetailDto>>;
public record GetContributionDetailsByMemberQuery(decimal MemberNo) : IRequest<IReadOnlyList<ContributionDetailDto>>;

public class GetContributionDetailsByBatchHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetContributionDetailsByBatchQuery, IReadOnlyList<ContributionDetailDto>>
{
    public async Task<IReadOnlyList<ContributionDetailDto>> Handle(GetContributionDetailsByBatchQuery request, CancellationToken ct)
    {
        var details = await uow.ContributionDetails.GetByBatchNoAsync(request.BatchNo, ct);
        return mapper.Map<IReadOnlyList<ContributionDetailDto>>(details);
    }
}

public class GetContributionDetailsByMemberHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetContributionDetailsByMemberQuery, IReadOnlyList<ContributionDetailDto>>
{
    public async Task<IReadOnlyList<ContributionDetailDto>> Handle(GetContributionDetailsByMemberQuery request, CancellationToken ct)
    {
        var details = await uow.ContributionDetails.GetByMemberNoAsync(request.MemberNo, ct);
        return mapper.Map<IReadOnlyList<ContributionDetailDto>>(details);
    }
}
