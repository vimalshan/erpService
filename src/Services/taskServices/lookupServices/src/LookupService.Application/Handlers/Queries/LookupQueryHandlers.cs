using AutoMapper;
using LookupService.Application.DTOs;
using LookupService.Application.Queries;
using LookupService.Domain.Interfaces;
using MediatR;

namespace LookupService.Application.Handlers.Queries;

public class GetAllLovTypesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllLovTypesQuery, IEnumerable<LovTypeMasterDto>>
{
    public async Task<IEnumerable<LovTypeMasterDto>> Handle(GetAllLovTypesQuery request, CancellationToken ct)
    {
        var entities = await uow.LovTypeMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<LovTypeMasterDto>>(entities);
    }
}

public class GetLovTypeByCodeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetLovTypeByCodeQuery, LovTypeMasterDto?>
{
    public async Task<LovTypeMasterDto?> Handle(GetLovTypeByCodeQuery request, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasters.GetByCodeAsync(request.TypeCode, ct);
        return entity is null ? null : mapper.Map<LovTypeMasterDto>(entity);
    }
}

public class GetAllLovsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllLovsQuery, IEnumerable<LovMasterDto>>
{
    public async Task<IEnumerable<LovMasterDto>> Handle(GetAllLovsQuery request, CancellationToken ct)
    {
        var entities = await uow.LovMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<LovMasterDto>>(entities);
    }
}

public class GetLovByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetLovByIdQuery, LovMasterDto?>
{
    public async Task<LovMasterDto?> Handle(GetLovByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(request.LovId, ct);
        return entity is null ? null : mapper.Map<LovMasterDto>(entity);
    }
}

public class GetLovsByTypeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetLovsByTypeQuery, IEnumerable<LovMasterDto>>
{
    public async Task<IEnumerable<LovMasterDto>> Handle(GetLovsByTypeQuery request, CancellationToken ct)
    {
        var entities = await uow.LovMasters.GetByTypeAsync(request.LovType, ct);
        return mapper.Map<IEnumerable<LovMasterDto>>(entities);
    }
}

public class GetAllProcessesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllProcessesQuery, IEnumerable<ProcessMasterDto>>
{
    public async Task<IEnumerable<ProcessMasterDto>> Handle(GetAllProcessesQuery request, CancellationToken ct)
    {
        var entities = await uow.ProcessMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ProcessMasterDto>>(entities);
    }
}

public class GetProcessByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetProcessByIdQuery, ProcessMasterDto?>
{
    public async Task<ProcessMasterDto?> Handle(GetProcessByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.ProcessMasters.GetByIdAsync(request.ProcessId, ct);
        return entity is null ? null : mapper.Map<ProcessMasterDto>(entity);
    }
}

public class GetAllPanelsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllPanelsQuery, IEnumerable<PanelMasterDto>>
{
    public async Task<IEnumerable<PanelMasterDto>> Handle(GetAllPanelsQuery request, CancellationToken ct)
    {
        var entities = await uow.PanelMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<PanelMasterDto>>(entities);
    }
}

public class GetPanelByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetPanelByIdQuery, PanelMasterDto?>
{
    public async Task<PanelMasterDto?> Handle(GetPanelByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.PanelMasters.GetByIdAsync(request.PanelId, ct);
        return entity is null ? null : mapper.Map<PanelMasterDto>(entity);
    }
}

public class GetUnitProcessesByUnitCodeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetUnitProcessesByUnitCodeQuery, IEnumerable<UnitProcessMapDto>>
{
    public async Task<IEnumerable<UnitProcessMapDto>> Handle(GetUnitProcessesByUnitCodeQuery request, CancellationToken ct)
    {
        var entities = await uow.UnitProcessMaps.GetByUnitCodeAsync(request.UnitCode, ct);
        return mapper.Map<IEnumerable<UnitProcessMapDto>>(entities);
    }
}

public class GetLovUnitMapsByLovIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetLovUnitMapsByLovIdQuery, IEnumerable<LovUnitMapDto>>
{
    public async Task<IEnumerable<LovUnitMapDto>> Handle(GetLovUnitMapsByLovIdQuery request, CancellationToken ct)
    {
        var entities = await uow.LovUnitMaps.GetByLovIdAsync(request.LovId, ct);
        return mapper.Map<IEnumerable<LovUnitMapDto>>(entities);
    }
}

public class GetAllAccessMastersHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllAccessMastersQuery, IEnumerable<UnitLovAccessMasterDto>>
{
    public async Task<IEnumerable<UnitLovAccessMasterDto>> Handle(GetAllAccessMastersQuery request, CancellationToken ct)
    {
        var entities = await uow.UnitLovAccessMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<UnitLovAccessMasterDto>>(entities);
    }
}

public class GetAccessMasterByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAccessMasterByIdQuery, UnitLovAccessMasterDto?>
{
    public async Task<UnitLovAccessMasterDto?> Handle(GetAccessMasterByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.UnitLovAccessMasters.GetByIdAsync(request.AccessMastId, ct);
        return entity is null ? null : mapper.Map<UnitLovAccessMasterDto>(entity);
    }
}
