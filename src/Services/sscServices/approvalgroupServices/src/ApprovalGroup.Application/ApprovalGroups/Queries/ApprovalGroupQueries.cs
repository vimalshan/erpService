using MediatR;
using ApprovalGroup.Application.DTOs;
using AutoMapper;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Domain.Exceptions;

namespace ApprovalGroup.Application.ApprovalGroups.Queries;

// ─── Get by ID ───────────────────────────────────────────────
public record GetApprovalGroupByIdQuery(long GroupId) : IRequest<ApprovalGroupDto>;

public class GetApprovalGroupByIdHandler : IRequestHandler<GetApprovalGroupByIdQuery, ApprovalGroupDto>
{
    private readonly IApprovalGroupRepository _repo;
    private readonly IMapper _mapper;

    public GetApprovalGroupByIdHandler(IApprovalGroupRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ApprovalGroupDto> Handle(GetApprovalGroupByIdQuery request, CancellationToken ct)
    {
        var group = await _repo.GetByIdAsync(request.GroupId, ct)
            ?? throw new ApprovalGroupNotFoundException(request.GroupId);
        return _mapper.Map<ApprovalGroupDto>(group);
    }
}

// ─── Get All ─────────────────────────────────────────────────
public record GetAllApprovalGroupsQuery : IRequest<IEnumerable<ApprovalGroupDto>>;

public class GetAllApprovalGroupsHandler : IRequestHandler<GetAllApprovalGroupsQuery, IEnumerable<ApprovalGroupDto>>
{
    private readonly IApprovalGroupRepository _repo;
    private readonly IMapper _mapper;

    public GetAllApprovalGroupsHandler(IApprovalGroupRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApprovalGroupDto>> Handle(GetAllApprovalGroupsQuery request, CancellationToken ct)
    {
        var groups = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ApprovalGroupDto>>(groups);
    }
}
