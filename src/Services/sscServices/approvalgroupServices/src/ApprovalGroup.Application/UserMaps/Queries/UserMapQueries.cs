using MediatR;
using AutoMapper;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Application.UserMaps.Queries;

public record GetUserMapsByGroupIdQuery(long GroupId) : IRequest<IEnumerable<ApprovalGroupUserMapDto>>;

public class GetUserMapsByGroupIdHandler : IRequestHandler<GetUserMapsByGroupIdQuery, IEnumerable<ApprovalGroupUserMapDto>>
{
    private readonly IApprovalGroupUserMapRepository _repo;
    private readonly IMapper _mapper;

    public GetUserMapsByGroupIdHandler(IApprovalGroupUserMapRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApprovalGroupUserMapDto>> Handle(GetUserMapsByGroupIdQuery request, CancellationToken ct)
    {
        var userMaps = await _repo.GetByGroupIdAsync(request.GroupId, ct);
        return _mapper.Map<IEnumerable<ApprovalGroupUserMapDto>>(userMaps);
    }
}
