using MediatR;
using AutoMapper;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Domain.Exceptions;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Application.PullMatrix.Queries;

public record GetPullMatrixByIdQuery(long MatId) : IRequest<PullMatrixDetailDto>;

public class GetPullMatrixByIdHandler : IRequestHandler<GetPullMatrixByIdQuery, PullMatrixDetailDto>
{
    private readonly IPullMatrixRepository _repo;
    private readonly IMapper _mapper;

    public GetPullMatrixByIdHandler(IPullMatrixRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PullMatrixDetailDto> Handle(GetPullMatrixByIdQuery request, CancellationToken ct)
    {
        var detail = await _repo.GetByIdAsync(request.MatId, ct)
            ?? throw new PullMatrixNotFoundException(request.MatId);
        return _mapper.Map<PullMatrixDetailDto>(detail);
    }
}

public record GetPullMatrixByUnitIdQuery(long UnitId) : IRequest<IEnumerable<PullMatrixDetailDto>>;

public class GetPullMatrixByUnitIdHandler : IRequestHandler<GetPullMatrixByUnitIdQuery, IEnumerable<PullMatrixDetailDto>>
{
    private readonly IPullMatrixRepository _repo;
    private readonly IMapper _mapper;

    public GetPullMatrixByUnitIdHandler(IPullMatrixRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PullMatrixDetailDto>> Handle(GetPullMatrixByUnitIdQuery request, CancellationToken ct)
    {
        var details = await _repo.GetByUnitIdAsync(request.UnitId, ct);
        return _mapper.Map<IEnumerable<PullMatrixDetailDto>>(details);
    }
}
