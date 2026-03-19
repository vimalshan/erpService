using AutoMapper;
using MediatR;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Commands;
using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;

namespace MasterDataService.Application.Handlers;

// ============ LOV Master Handlers ============
public class CreateLovMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateLovMasterCommand, LovMasterDto>
{
    public async Task<LovMasterDto> Handle(CreateLovMasterCommand request, CancellationToken ct)
    {
        var entity = LovMaster.Create(request.LovId, request.LovType, request.LovName);
        await uow.LovMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<LovMasterDto>(entity);
    }
}

public class UpdateLovMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateLovMasterCommand, LovMasterDto>
{
    public async Task<LovMasterDto> Handle(UpdateLovMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(request.LovId, ct)
            ?? throw new KeyNotFoundException($"LOV Master with ID {request.LovId} not found.");
        entity.Update(request.LovType, request.LovName);
        uow.LovMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<LovMasterDto>(entity);
    }
}

public class DeleteLovMasterHandler(IUnitOfWork uow) : IRequestHandler<DeleteLovMasterCommand, bool>
{
    public async Task<bool> Handle(DeleteLovMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(request.LovId, ct);
        if (entity is null) return false;
        uow.LovMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// ============ LOV Type Master Handlers ============
public class CreateLovTypeMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateLovTypeMasterCommand, LovTypeMasterDto>
{
    public async Task<LovTypeMasterDto> Handle(CreateLovTypeMasterCommand request, CancellationToken ct)
    {
        var entity = LovTypeMaster.Create(request.TypeCode, request.TypeName);
        await uow.LovTypeMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<LovTypeMasterDto>(entity);
    }
}

public class UpdateLovTypeMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateLovTypeMasterCommand, LovTypeMasterDto>
{
    public async Task<LovTypeMasterDto> Handle(UpdateLovTypeMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasters.GetByIdAsync(request.TypeCode, ct)
            ?? throw new KeyNotFoundException($"LOV Type Master '{request.TypeCode}' not found.");
        entity.Update(request.TypeName);
        uow.LovTypeMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<LovTypeMasterDto>(entity);
    }
}

public class DeleteLovTypeMasterHandler(IUnitOfWork uow) : IRequestHandler<DeleteLovTypeMasterCommand, bool>
{
    public async Task<bool> Handle(DeleteLovTypeMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasters.GetByIdAsync(request.TypeCode, ct);
        if (entity is null) return false;
        uow.LovTypeMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// ============ Hold Type Master Handlers ============
public class CreateHoldTypeMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateHoldTypeMasterCommand, HoldTypeMasterDto>
{
    public async Task<HoldTypeMasterDto> Handle(CreateHoldTypeMasterCommand request, CancellationToken ct)
    {
        var entity = HoldTypeMaster.Create(request.HoldId, request.HoldName, request.HoldCategory);
        await uow.HoldTypeMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<HoldTypeMasterDto>(entity);
    }
}

public class UpdateHoldTypeMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateHoldTypeMasterCommand, HoldTypeMasterDto>
{
    public async Task<HoldTypeMasterDto> Handle(UpdateHoldTypeMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.HoldTypeMasters.GetByIdAsync(request.HoldId, ct)
            ?? throw new KeyNotFoundException($"Hold Type Master with ID {request.HoldId} not found.");
        entity.Update(request.HoldName, request.HoldCategory);
        uow.HoldTypeMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<HoldTypeMasterDto>(entity);
    }
}

public class DeleteHoldTypeMasterHandler(IUnitOfWork uow) : IRequestHandler<DeleteHoldTypeMasterCommand, bool>
{
    public async Task<bool> Handle(DeleteHoldTypeMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.HoldTypeMasters.GetByIdAsync(request.HoldId, ct);
        if (entity is null) return false;
        uow.HoldTypeMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// ============ Location Scan Param Handlers ============
public class CreateLocationScanParamHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateLocationScanParamCommand, LocationScanParamDto>
{
    public async Task<LocationScanParamDto> Handle(CreateLocationScanParamCommand request, CancellationToken ct)
    {
        var entity = LocationScanParam.Create(request.ParamId, request.LocationId, request.EffectiveDate, request.ClosingDate);
        await uow.LocationScanParams.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<LocationScanParamDto>(entity);
    }
}

public class UpdateLocationScanParamHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateLocationScanParamCommand, LocationScanParamDto>
{
    public async Task<LocationScanParamDto> Handle(UpdateLocationScanParamCommand request, CancellationToken ct)
    {
        var entity = await uow.LocationScanParams.GetByIdAsync(request.ParamId, ct)
            ?? throw new KeyNotFoundException($"Location Scan Param with ID {request.ParamId} not found.");
        entity.UpdatePeriod(request.EffectiveDate, request.ClosingDate);
        uow.LocationScanParams.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<LocationScanParamDto>(entity);
    }
}

public class DeleteLocationScanParamHandler(IUnitOfWork uow) : IRequestHandler<DeleteLocationScanParamCommand, bool>
{
    public async Task<bool> Handle(DeleteLocationScanParamCommand request, CancellationToken ct)
    {
        var entity = await uow.LocationScanParams.GetByIdAsync(request.ParamId, ct);
        if (entity is null) return false;
        uow.LocationScanParams.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// ============ Scanner Master Handlers ============
public class CreateScannerMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateScannerMasterCommand, ScannerMasterDto>
{
    public async Task<ScannerMasterDto> Handle(CreateScannerMasterCommand request, CancellationToken ct)
    {
        var entity = ScannerMaster.Create(request.DeviceId, request.DeviceName, request.DeviceLocationId, request.DevicePath);
        await uow.ScannerMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScannerMasterDto>(entity);
    }
}

public class UpdateScannerMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateScannerMasterCommand, ScannerMasterDto>
{
    public async Task<ScannerMasterDto> Handle(UpdateScannerMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.ScannerMasters.GetByIdAsync(request.DeviceId, ct)
            ?? throw new KeyNotFoundException($"Scanner Master with ID {request.DeviceId} not found.");
        entity.Update(request.DeviceName, request.DeviceLocationId, request.DevicePath);
        uow.ScannerMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScannerMasterDto>(entity);
    }
}

public class DeleteScannerMasterHandler(IUnitOfWork uow) : IRequestHandler<DeleteScannerMasterCommand, bool>
{
    public async Task<bool> Handle(DeleteScannerMasterCommand request, CancellationToken ct)
    {
        var entity = await uow.ScannerMasters.GetByIdAsync(request.DeviceId, ct);
        if (entity is null) return false;
        uow.ScannerMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
