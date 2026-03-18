using AutoMapper;
using MediatR;
using OrganizationStructureService.Application.Commands;
using OrganizationStructureService.Application.DTOs;
using OrganizationStructureService.Domain.Entities;
using OrganizationStructureService.Domain.Exceptions;
using OrganizationStructureService.Domain.Interfaces;

namespace OrganizationStructureService.Application.Handlers;

public class CreateBusinessCommandHandler : IRequestHandler<CreateBusinessCommand, BusinessDto>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public CreateBusinessCommandHandler(IBusinessRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<BusinessDto> Handle(CreateBusinessCommand request, CancellationToken ct)
    {
        var business = Business.Create(
            request.BusinessId, request.BusinessName, request.BusinessShortName,
            request.BusinessCode, request.CompanyId, request.CompanyCode, request.UpdatedBy);
        await _repo.AddAsync(business, ct);
        return _mapper.Map<BusinessDto>(business);
    }
}

public class UpdateBusinessCommandHandler : IRequestHandler<UpdateBusinessCommand, BusinessDto>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public UpdateBusinessCommandHandler(IBusinessRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<BusinessDto> Handle(UpdateBusinessCommand request, CancellationToken ct)
    {
        var business = await _repo.GetByIdAsync(request.BusinessId, ct)
            ?? throw new BusinessNotFoundException(request.BusinessId);
        business.Update(request.BusinessName, request.BusinessShortName, request.UpdatedBy);
        await _repo.UpdateAsync(business, ct);
        return _mapper.Map<BusinessDto>(business);
    }
}

public class DeactivateBusinessCommandHandler : IRequestHandler<DeactivateBusinessCommand, bool>
{
    private readonly IBusinessRepository _repo;
    public DeactivateBusinessCommandHandler(IBusinessRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeactivateBusinessCommand request, CancellationToken ct)
    {
        var business = await _repo.GetByIdAsync(request.BusinessId, ct)
            ?? throw new BusinessNotFoundException(request.BusinessId);
        business.Deactivate(request.UpdatedBy);
        await _repo.UpdateAsync(business, ct);
        return true;
    }
}

public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, UnitDto>
{
    private readonly IUnitRepository _repo;
    private readonly IMapper _mapper;

    public CreateUnitCommandHandler(IUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<UnitDto> Handle(CreateUnitCommand request, CancellationToken ct)
    {
        var unit = Domain.Entities.Unit.Create(
            request.UnitId, request.UnitName, request.UnitShortName, request.UnitCode,
            request.BusinessId, request.BusinessCode, request.OrgId, request.ReportFlag, request.UpdatedBy);
        await _repo.AddAsync(unit, ct);
        return _mapper.Map<UnitDto>(unit);
    }
}

public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, UnitDto>
{
    private readonly IUnitRepository _repo;
    private readonly IMapper _mapper;

    public UpdateUnitCommandHandler(IUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<UnitDto> Handle(UpdateUnitCommand request, CancellationToken ct)
    {
        var unit = await _repo.GetByIdAsync(request.UnitId, ct)
            ?? throw new UnitNotFoundException(request.UnitId);
        unit.Update(request.UnitName, request.UnitShortName, request.UpdatedBy);
        await _repo.UpdateAsync(unit, ct);
        return _mapper.Map<UnitDto>(unit);
    }
}

public class CreateGradeCommandHandler : IRequestHandler<CreateGradeCommand, GradeDto>
{
    private readonly IGradeRepository _repo;
    private readonly IMapper _mapper;

    public CreateGradeCommandHandler(IGradeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<GradeDto> Handle(CreateGradeCommand request, CancellationToken ct)
    {
        var grade = Grade.Create(
            request.GradeId, request.GradeName, request.GradeCode, request.GradeDesignation,
            request.CategoryCode, request.ManagementCategoryCode, request.Priority);
        await _repo.AddAsync(grade, ct);
        return _mapper.Map<GradeDto>(grade);
    }
}

public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, PositionDto>
{
    private readonly IPositionRepository _repo;
    private readonly IMapper _mapper;

    public CreatePositionCommandHandler(IPositionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<PositionDto> Handle(CreatePositionCommand request, CancellationToken ct)
    {
        var position = Position.Create(
            request.PositionId, request.UnitCode, request.GradeId, request.Designation,
            request.EffectiveDate, request.ReferenceCode, request.EnteredBy);
        await _repo.AddAsync(position, ct);
        return _mapper.Map<PositionDto>(position);
    }
}
