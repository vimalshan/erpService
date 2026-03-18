using AutoMapper;
using MediatR;
using OrganizationStructureService.Application.Commands;
using OrganizationStructureService.Application.DTOs;
using OrganizationStructureService.Application.Queries;
using OrganizationStructureService.Domain.Entities;
using OrganizationStructureService.Domain.Exceptions;
using OrganizationStructureService.Domain.Interfaces;

namespace OrganizationStructureService.Application.Handlers;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper _mapper;
    public CreateDepartmentCommandHandler(IDepartmentRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var dept = Department.Create(request.DepartmentId, request.DepartmentName, request.UpdatedBy);
        await _repo.AddAsync(dept, ct);
        return _mapper.Map<DepartmentDto>(dept);
    }
}

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper _mapper;
    public UpdateDepartmentCommandHandler(IDepartmentRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DepartmentDto> Handle(UpdateDepartmentCommand request, CancellationToken ct)
    {
        var dept = await _repo.GetByIdAsync(request.DepartmentId, ct)
            ?? throw new DepartmentNotFoundException(request.DepartmentId);
        dept.Update(request.DepartmentName, request.DepartmentCode, request.UpdatedBy);
        await _repo.UpdateAsync(dept, ct);
        return _mapper.Map<DepartmentDto>(dept);
    }
}

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper _mapper;
    public GetAllDepartmentsQueryHandler(IDepartmentRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
    {
        var departments = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
    }
}

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto?>
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper _mapper;
    public GetDepartmentByIdQueryHandler(IDepartmentRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DepartmentDto?> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
    {
        var dept = await _repo.GetByIdAsync(request.DepartmentId, ct);
        return dept is null ? null : _mapper.Map<DepartmentDto>(dept);
    }
}

public class CreateDivisionCommandHandler : IRequestHandler<CreateDivisionCommand, DivisionDto>
{
    private readonly IDivisionRepository _repo;
    private readonly IMapper _mapper;
    public CreateDivisionCommandHandler(IDivisionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DivisionDto> Handle(CreateDivisionCommand request, CancellationToken ct)
    {
        var division = Division.Create(request.DivisionId, request.DivisionName, request.DivisionCode, request.UpdatedBy);
        await _repo.AddAsync(division, ct);
        return _mapper.Map<DivisionDto>(division);
    }
}

public class GetAllDivisionsQueryHandler : IRequestHandler<GetAllDivisionsQuery, IReadOnlyList<DivisionDto>>
{
    private readonly IDivisionRepository _repo;
    private readonly IMapper _mapper;
    public GetAllDivisionsQueryHandler(IDivisionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<DivisionDto>> Handle(GetAllDivisionsQuery request, CancellationToken ct)
    {
        var divisions = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<DivisionDto>>(divisions);
    }
}

public class GetDivisionByIdQueryHandler : IRequestHandler<GetDivisionByIdQuery, DivisionDto?>
{
    private readonly IDivisionRepository _repo;
    private readonly IMapper _mapper;
    public GetDivisionByIdQueryHandler(IDivisionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DivisionDto?> Handle(GetDivisionByIdQuery request, CancellationToken ct)
    {
        var div = await _repo.GetByIdAsync(request.DivisionId, ct);
        return div is null ? null : _mapper.Map<DivisionDto>(div);
    }
}

public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, GradeDto>
{
    private readonly IGradeRepository _repo;
    private readonly IMapper _mapper;
    public UpdateGradeCommandHandler(IGradeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<GradeDto> Handle(UpdateGradeCommand request, CancellationToken ct)
    {
        var grade = await _repo.GetByIdAsync(request.GradeId, ct)
            ?? throw new GradeNotFoundException(request.GradeId);
        grade.Update(request.GradeName, request.GradeDesignation, request.Priority);
        await _repo.UpdateAsync(grade, ct);
        return _mapper.Map<GradeDto>(grade);
    }
}

public class GetGradeByIdQueryHandler : IRequestHandler<GetGradeByIdQuery, GradeDto?>
{
    private readonly IGradeRepository _repo;
    private readonly IMapper _mapper;
    public GetGradeByIdQueryHandler(IGradeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<GradeDto?> Handle(GetGradeByIdQuery request, CancellationToken ct)
    {
        var grade = await _repo.GetByIdAsync(request.GradeId, ct);
        return grade is null ? null : _mapper.Map<GradeDto>(grade);
    }
}

public class GetActiveGradesQueryHandler : IRequestHandler<GetActiveGradesQuery, IReadOnlyList<GradeDto>>
{
    private readonly IGradeRepository _repo;
    private readonly IMapper _mapper;
    public GetActiveGradesQueryHandler(IGradeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<GradeDto>> Handle(GetActiveGradesQuery request, CancellationToken ct)
    {
        var grades = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IReadOnlyList<GradeDto>>(grades);
    }
}

public class GetActiveUnitsQueryHandler : IRequestHandler<GetActiveUnitsQuery, IReadOnlyList<UnitDto>>
{
    private readonly IUnitRepository _repo;
    private readonly IMapper _mapper;
    public GetActiveUnitsQueryHandler(IUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<UnitDto>> Handle(GetActiveUnitsQuery request, CancellationToken ct)
    {
        var units = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IReadOnlyList<UnitDto>>(units);
    }
}

public class DeactivateUnitCommandHandler : IRequestHandler<DeactivateUnitCommand, bool>
{
    private readonly IUnitRepository _repo;
    public DeactivateUnitCommandHandler(IUnitRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeactivateUnitCommand request, CancellationToken ct)
    {
        var unit = await _repo.GetByIdAsync(request.UnitId, ct)
            ?? throw new UnitNotFoundException(request.UnitId);
        unit.Deactivate(request.UpdatedBy);
        await _repo.UpdateAsync(unit, ct);
        return true;
    }
}

public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, PositionDto?>
{
    private readonly IPositionRepository _repo;
    private readonly IMapper _mapper;
    public GetPositionByIdQueryHandler(IPositionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<PositionDto?> Handle(GetPositionByIdQuery request, CancellationToken ct)
    {
        var pos = await _repo.GetByIdAsync(request.PositionId, ct);
        return pos is null ? null : _mapper.Map<PositionDto>(pos);
    }
}

public class ClosePositionCommandHandler : IRequestHandler<ClosePositionCommand, bool>
{
    private readonly IPositionRepository _repo;
    public ClosePositionCommandHandler(IPositionRepository repo) => _repo = repo;

    public async Task<bool> Handle(ClosePositionCommand request, CancellationToken ct)
    {
        var pos = await _repo.GetByIdAsync(request.PositionId, ct)
            ?? throw new PositionNotFoundException(request.PositionId);
        pos.Close(request.CloseDate, request.ModifiedBy);
        await _repo.UpdateAsync(pos, ct);
        return true;
    }
}

public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, bool>
{
    private readonly IPositionRepository _repo;
    public DeletePositionCommandHandler(IPositionRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeletePositionCommand request, CancellationToken ct)
    {
        var pos = await _repo.GetByIdAsync(request.PositionId, ct)
            ?? throw new PositionNotFoundException(request.PositionId);
        pos.MarkDeleted(request.ModifiedBy);
        await _repo.UpdateAsync(pos, ct);
        return true;
    }
}

public class GetSiteByIdQueryHandler : IRequestHandler<GetSiteByIdQuery, SiteDto?>
{
    private readonly ISiteRepository _repo;
    private readonly IMapper _mapper;
    public GetSiteByIdQueryHandler(ISiteRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<SiteDto?> Handle(GetSiteByIdQuery request, CancellationToken ct)
    {
        var site = await _repo.GetByIdAsync(request.SiteId, ct);
        return site is null ? null : _mapper.Map<SiteDto>(site);
    }
}

public class CreateSiteCommandHandler : IRequestHandler<CreateSiteCommand, SiteDto>
{
    private readonly ISiteRepository _repo;
    private readonly IMapper _mapper;
    public CreateSiteCommandHandler(ISiteRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<SiteDto> Handle(CreateSiteCommand request, CancellationToken ct)
    {
        var site = Site.Create(request.SiteId, request.SiteName, request.SiteShortName, request.CityCode, request.CategoryCode);
        await _repo.AddAsync(site, ct);
        return _mapper.Map<SiteDto>(site);
    }
}
