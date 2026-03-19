using AutoMapper;
using MediatR;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries;
using MasterDataService.Domain.Interfaces;

namespace MasterDataService.Application.Handlers;

// ============ LOV Master Query Handlers ============
public class GetAllLovMastersHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllLovMastersQuery, IReadOnlyList<LovMasterDto>>
{
    public async Task<IReadOnlyList<LovMasterDto>> Handle(GetAllLovMastersQuery request, CancellationToken ct)
    {
        var entities = await uow.LovMasters.GetAllAsync(ct);
        return entities.Select(mapper.Map<LovMasterDto>).ToList();
    }
}

public class GetLovMasterByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetLovMasterByIdQuery, LovMasterDto?>
{
    public async Task<LovMasterDto?> Handle(GetLovMasterByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(request.LovId, ct);
        return entity is null ? null : mapper.Map<LovMasterDto>(entity);
    }
}

public class GetLovMastersByTypeHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetLovMastersByTypeQuery, IReadOnlyList<LovMasterDto>>
{
    public async Task<IReadOnlyList<LovMasterDto>> Handle(GetLovMastersByTypeQuery request, CancellationToken ct)
    {
        var entities = await uow.LovMasters.GetByTypeAsync(request.LovType, ct);
        return entities.Select(mapper.Map<LovMasterDto>).ToList();
    }
}

// ============ LOV Type Master Query Handlers ============
public class GetAllLovTypeMastersHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllLovTypeMastersQuery, IReadOnlyList<LovTypeMasterDto>>
{
    public async Task<IReadOnlyList<LovTypeMasterDto>> Handle(GetAllLovTypeMastersQuery request, CancellationToken ct)
    {
        var entities = await uow.LovTypeMasters.GetAllAsync(ct);
        return entities.Select(mapper.Map<LovTypeMasterDto>).ToList();
    }
}

public class GetLovTypeMasterByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetLovTypeMasterByIdQuery, LovTypeMasterDto?>
{
    public async Task<LovTypeMasterDto?> Handle(GetLovTypeMasterByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasters.GetByIdAsync(request.TypeCode, ct);
        return entity is null ? null : mapper.Map<LovTypeMasterDto>(entity);
    }
}

// ============ Hold Type Master Query Handlers ============
public class GetAllHoldTypeMastersHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllHoldTypeMastersQuery, IReadOnlyList<HoldTypeMasterDto>>
{
    public async Task<IReadOnlyList<HoldTypeMasterDto>> Handle(GetAllHoldTypeMastersQuery request, CancellationToken ct)
    {
        var entities = await uow.HoldTypeMasters.GetAllAsync(ct);
        return entities.Select(mapper.Map<HoldTypeMasterDto>).ToList();
    }
}

public class GetHoldTypeMasterByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetHoldTypeMasterByIdQuery, HoldTypeMasterDto?>
{
    public async Task<HoldTypeMasterDto?> Handle(GetHoldTypeMasterByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.HoldTypeMasters.GetByIdAsync(request.HoldId, ct);
        return entity is null ? null : mapper.Map<HoldTypeMasterDto>(entity);
    }
}

// ============ Location Scan Param Query Handlers ============
public class GetAllLocationScanParamsHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllLocationScanParamsQuery, IReadOnlyList<LocationScanParamDto>>
{
    public async Task<IReadOnlyList<LocationScanParamDto>> Handle(GetAllLocationScanParamsQuery request, CancellationToken ct)
    {
        var entities = await uow.LocationScanParams.GetAllAsync(ct);
        return entities.Select(mapper.Map<LocationScanParamDto>).ToList();
    }
}

public class GetLocationScanParamByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetLocationScanParamByIdQuery, LocationScanParamDto?>
{
    public async Task<LocationScanParamDto?> Handle(GetLocationScanParamByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.LocationScanParams.GetByIdAsync(request.ParamId, ct);
        return entity is null ? null : mapper.Map<LocationScanParamDto>(entity);
    }
}

// ============ Scanner Master Query Handlers ============
public class GetAllScannerMastersHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllScannerMastersQuery, IReadOnlyList<ScannerMasterDto>>
{
    public async Task<IReadOnlyList<ScannerMasterDto>> Handle(GetAllScannerMastersQuery request, CancellationToken ct)
    {
        var entities = await uow.ScannerMasters.GetAllAsync(ct);
        return entities.Select(mapper.Map<ScannerMasterDto>).ToList();
    }
}

public class GetScannerMasterByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetScannerMasterByIdQuery, ScannerMasterDto?>
{
    public async Task<ScannerMasterDto?> Handle(GetScannerMasterByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.ScannerMasters.GetByIdAsync(request.DeviceId, ct);
        return entity is null ? null : mapper.Map<ScannerMasterDto>(entity);
    }
}
