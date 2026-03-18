using AutoMapper;
using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Queries.GetGroupIncentives;

public record GetGroupIncentivesQuery(int GroupId) : IRequest<IEnumerable<GroupIncentiveMainDto>>;
public record GetPendingIncentivesQuery() : IRequest<IEnumerable<GroupIncentiveMainDto>>;

public class GetGroupIncentivesHandler : IRequestHandler<GetGroupIncentivesQuery, IEnumerable<GroupIncentiveMainDto>>
{
    private readonly IGroupIncentiveMainRepository _repository;
    private readonly IMapper _mapper;

    public GetGroupIncentivesHandler(IGroupIncentiveMainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupIncentiveMainDto>> Handle(GetGroupIncentivesQuery request, CancellationToken cancellationToken)
    {
        var incentives = await _repository.GetByGroupIdAsync(request.GroupId, cancellationToken);
        return _mapper.Map<IEnumerable<GroupIncentiveMainDto>>(incentives);
    }
}

public class GetPendingIncentivesHandler : IRequestHandler<GetPendingIncentivesQuery, IEnumerable<GroupIncentiveMainDto>>
{
    private readonly IGroupIncentiveMainRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingIncentivesHandler(IGroupIncentiveMainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupIncentiveMainDto>> Handle(GetPendingIncentivesQuery request, CancellationToken cancellationToken)
    {
        var incentives = await _repository.GetPendingAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GroupIncentiveMainDto>>(incentives);
    }
}
