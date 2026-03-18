using AutoMapper;
using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Domain.Interfaces;
using MediatR;

namespace GSTComplianceService.Application.Features.GstMain.Queries;

// ── Get by ID ─────────────────────────────────────────────────────
public record GetGstDetailsQuery(long GstId) : IRequest<GstMainDto>;

public class GetGstDetailsQueryHandler : IRequestHandler<GetGstDetailsQuery, GstMainDto>
{
    private readonly IGstMainRepository _repository;
    private readonly IMapper _mapper;

    public GetGstDetailsQueryHandler(IGstMainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GstMainDto> Handle(GetGstDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.GstId, cancellationToken)
            ?? throw new Common.Exceptions.NotFoundException(nameof(Domain.Entities.GstMain), request.GstId);
        return _mapper.Map<GstMainDto>(entity);
    }
}

// ── Get by PAN ────────────────────────────────────────────────────
public record GetGstByPanQuery(string PanNo) : IRequest<GstMainDto?>;

public class GetGstByPanQueryHandler : IRequestHandler<GetGstByPanQuery, GstMainDto?>
{
    private readonly IGstMainRepository _repository;
    private readonly IMapper _mapper;

    public GetGstByPanQueryHandler(IGstMainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GstMainDto?> Handle(GetGstByPanQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByPanNoAsync(request.PanNo, cancellationToken);
        return entity is null ? null : _mapper.Map<GstMainDto>(entity);
    }
}

// ── Get All Paged ─────────────────────────────────────────────────
public record GetAllGstQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<GstMainDto>>;

public class GetAllGstQueryHandler : IRequestHandler<GetAllGstQuery, PagedResult<GstMainDto>>
{
    private readonly IGstMainRepository _repository;
    private readonly IMapper _mapper;

    public GetAllGstQueryHandler(IGstMainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GstMainDto>> Handle(GetAllGstQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        var total = await _repository.GetTotalCountAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<GstMainDto>>(items);
        return new PagedResult<GstMainDto>(dtos, request.Page, request.PageSize, total);
    }
}
