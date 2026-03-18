using AutoMapper;
using MediatR;
using OrganizationStructureService.Application.DTOs;
using OrganizationStructureService.Application.Queries;
using OrganizationStructureService.Domain.Interfaces;

namespace OrganizationStructureService.Application.Handlers;

public class GetBusinessByIdQueryHandler : IRequestHandler<GetBusinessByIdQuery, BusinessDto?>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public GetBusinessByIdQueryHandler(IBusinessRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<BusinessDto?> Handle(GetBusinessByIdQuery request, CancellationToken ct)
    {
        var business = await _repo.GetByIdAsync(request.BusinessId, ct);
        return business is null ? null : _mapper.Map<BusinessDto>(business);
    }
}

public class GetAllBusinessesQueryHandler : IRequestHandler<GetAllBusinessesQuery, IReadOnlyList<BusinessDto>>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public GetAllBusinessesQueryHandler(IBusinessRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<BusinessDto>> Handle(GetAllBusinessesQuery request, CancellationToken ct)
    {
        var businesses = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<BusinessDto>>(businesses);
    }
}

public class GetActiveBusinessesQueryHandler : IRequestHandler<GetActiveBusinessesQuery, IReadOnlyList<BusinessDto>>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public GetActiveBusinessesQueryHandler(IBusinessRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<BusinessDto>> Handle(GetActiveBusinessesQuery request, CancellationToken ct)
    {
        var businesses = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IReadOnlyList<BusinessDto>>(businesses);
    }
}

public class GetUnitByIdQueryHandler : IRequestHandler<GetUnitByIdQuery, UnitDto?>
{
    private readonly IUnitRepository _repo;
    private readonly IMapper _mapper;

    public GetUnitByIdQueryHandler(IUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<UnitDto?> Handle(GetUnitByIdQuery request, CancellationToken ct)
    {
        var unit = await _repo.GetByIdAsync(request.UnitId, ct);
        return unit is null ? null : _mapper.Map<UnitDto>(unit);
    }
}

public class GetAllUnitsQueryHandler : IRequestHandler<GetAllUnitsQuery, IReadOnlyList<UnitDto>>
{
    private readonly IUnitRepository _repo;
    private readonly IMapper _mapper;

    public GetAllUnitsQueryHandler(IUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<UnitDto>> Handle(GetAllUnitsQuery request, CancellationToken ct)
    {
        var units = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<UnitDto>>(units);
    }
}

public class GetUnitsByBusinessIdQueryHandler : IRequestHandler<GetUnitsByBusinessIdQuery, IReadOnlyList<UnitDto>>
{
    private readonly IUnitRepository _repo;
    private readonly IMapper _mapper;

    public GetUnitsByBusinessIdQueryHandler(IUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<UnitDto>> Handle(GetUnitsByBusinessIdQuery request, CancellationToken ct)
    {
        var units = await _repo.GetByBusinessIdAsync(request.BusinessId, ct);
        return _mapper.Map<IReadOnlyList<UnitDto>>(units);
    }
}

public class GetAllGradesQueryHandler : IRequestHandler<GetAllGradesQuery, IReadOnlyList<GradeDto>>
{
    private readonly IGradeRepository _repo;
    private readonly IMapper _mapper;

    public GetAllGradesQueryHandler(IGradeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<GradeDto>> Handle(GetAllGradesQuery request, CancellationToken ct)
    {
        var grades = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<GradeDto>>(grades);
    }
}

public class GetAllPositionsQueryHandler : IRequestHandler<GetAllPositionsQuery, IReadOnlyList<PositionDto>>
{
    private readonly IPositionRepository _repo;
    private readonly IMapper _mapper;

    public GetAllPositionsQueryHandler(IPositionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<PositionDto>> Handle(GetAllPositionsQuery request, CancellationToken ct)
    {
        var positions = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<PositionDto>>(positions);
    }
}

public class GetPositionsByUnitCodeQueryHandler : IRequestHandler<GetPositionsByUnitCodeQuery, IReadOnlyList<PositionDto>>
{
    private readonly IPositionRepository _repo;
    private readonly IMapper _mapper;

    public GetPositionsByUnitCodeQueryHandler(IPositionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<PositionDto>> Handle(GetPositionsByUnitCodeQuery request, CancellationToken ct)
    {
        var positions = await _repo.GetByUnitCodeAsync(request.UnitCode, ct);
        return _mapper.Map<IReadOnlyList<PositionDto>>(positions);
    }
}

public class GetAllSitesQueryHandler : IRequestHandler<GetAllSitesQuery, IReadOnlyList<SiteDto>>
{
    private readonly ISiteRepository _repo;
    private readonly IMapper _mapper;

    public GetAllSitesQueryHandler(ISiteRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<SiteDto>> Handle(GetAllSitesQuery request, CancellationToken ct)
    {
        var sites = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<SiteDto>>(sites);
    }
}
