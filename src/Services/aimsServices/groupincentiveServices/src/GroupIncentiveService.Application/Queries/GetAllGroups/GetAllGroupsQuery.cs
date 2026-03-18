using AutoMapper;
using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Queries.GetAllGroups;

public record GetAllGroupsQuery(bool ActiveOnly = true) : IRequest<IEnumerable<GroupMasterDto>>;

public class GetAllGroupsHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupMasterDto>>
{
    private readonly IGroupMasterRepository _repository;
    private readonly IMapper _mapper;

    public GetAllGroupsHandler(IGroupMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupMasterDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _repository.GetAllAsync(request.ActiveOnly, cancellationToken);
        return _mapper.Map<IEnumerable<GroupMasterDto>>(groups);
    }
}
