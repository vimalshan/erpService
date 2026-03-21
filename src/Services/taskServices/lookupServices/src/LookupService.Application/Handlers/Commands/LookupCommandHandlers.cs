using LookupService.Application.Commands;
using LookupService.Domain.Entities;
using LookupService.Domain.Interfaces;
using MediatR;

namespace LookupService.Application.Handlers.Commands;

public class CreateLovTypeHandler(IUnitOfWork uow)
    : IRequestHandler<CreateLovTypeCommand, string>
{
    public async Task<string> Handle(CreateLovTypeCommand request, CancellationToken ct)
    {
        var entity = LovTypeMaster.Create(request.LovTypeCode, request.LovTypeName);
        await uow.LovTypeMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.LovTypeCode;
    }
}

public class UpdateLovTypeHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateLovTypeCommand, bool>
{
    public async Task<bool> Handle(UpdateLovTypeCommand request, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasters.GetByCodeAsync(request.LovTypeCode, ct);
        if (entity is null) return false;
        entity.UpdateName(request.LovTypeName);
        uow.LovTypeMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteLovTypeHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteLovTypeCommand, bool>
{
    public async Task<bool> Handle(DeleteLovTypeCommand request, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasters.GetByCodeAsync(request.LovTypeCode, ct);
        if (entity is null) return false;
        uow.LovTypeMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateLovHandler(IUnitOfWork uow)
    : IRequestHandler<CreateLovCommand, long>
{
    public async Task<long> Handle(CreateLovCommand request, CancellationToken ct)
    {
        var all = await uow.LovMasters.GetAllAsync(ct);
        var nextId = all.Any() ? all.Max(x => x.LovId) + 1 : 1;
        var entity = LovMaster.Create(nextId, request.LovType, request.LovName);
        await uow.LovMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.LovId;
    }
}

public class UpdateLovHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateLovCommand, bool>
{
    public async Task<bool> Handle(UpdateLovCommand request, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(request.LovId, ct);
        if (entity is null) return false;
        entity.UpdateName(request.LovName);
        uow.LovMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteLovHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteLovCommand, bool>
{
    public async Task<bool> Handle(DeleteLovCommand request, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(request.LovId, ct);
        if (entity is null) return false;
        uow.LovMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class MapLovToUnitHandler(IUnitOfWork uow)
    : IRequestHandler<MapLovToUnitCommand, decimal>
{
    public async Task<decimal> Handle(MapLovToUnitCommand request, CancellationToken ct)
    {
        var maps = await uow.LovUnitMaps.GetByLovIdAsync(request.LovId, ct);
        var nextId = maps.Any() ? maps.Max(x => x.LuMapId) + 1 : 1;
        var entity = LovUnitMap.Create(nextId, request.LovId, request.UnitCode, request.Flag);
        await uow.LovUnitMaps.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.LuMapId;
    }
}

public class CreateProcessHandler(IUnitOfWork uow)
    : IRequestHandler<CreateProcessCommand, decimal>
{
    public async Task<decimal> Handle(CreateProcessCommand request, CancellationToken ct)
    {
        var existing = await uow.ProcessMasters.GetByIdAsync(request.ProcessId, ct);
        if (existing is not null)
        {
            existing.Update(request.ProcessName, request.LiveFlag);
            uow.ProcessMasters.Update(existing);
        }
        else
        {
            var entity = ProcessMaster.Create(request.ProcessId, request.ProcessName, request.LiveFlag);
            await uow.ProcessMasters.AddAsync(entity, ct);
        }
        await uow.SaveChangesAsync(ct);
        return request.ProcessId;
    }
}

public class UpdateProcessHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateProcessCommand, bool>
{
    public async Task<bool> Handle(UpdateProcessCommand request, CancellationToken ct)
    {
        var entity = await uow.ProcessMasters.GetByIdAsync(request.ProcessId, ct);
        if (entity is null) return false;
        entity.Update(request.ProcessName, request.LiveFlag);
        uow.ProcessMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteProcessHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteProcessCommand, bool>
{
    public async Task<bool> Handle(DeleteProcessCommand request, CancellationToken ct)
    {
        var entity = await uow.ProcessMasters.GetByIdAsync(request.ProcessId, ct);
        if (entity is null) return false;
        uow.ProcessMasters.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class MapUnitProcessHandler(IUnitOfWork uow)
    : IRequestHandler<MapUnitProcessCommand, decimal>
{
    public async Task<decimal> Handle(MapUnitProcessCommand request, CancellationToken ct)
    {
        var maps = await uow.UnitProcessMaps.GetByUnitCodeAsync(request.UnitCode, ct);
        var nextId = maps.Any() ? maps.Max(x => x.UpMapId) + 1 : 1;
        var entity = UnitProcessMap.Create(nextId, request.UnitCode, request.ProcessId);
        await uow.UnitProcessMaps.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.UpMapId;
    }
}

public class CreatePanelHandler(IUnitOfWork uow)
    : IRequestHandler<CreatePanelCommand, decimal>
{
    public async Task<decimal> Handle(CreatePanelCommand request, CancellationToken ct)
    {
        var entity = PanelMaster.Create(request.PanelId, request.PanelName);
        await uow.PanelMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.PanelId;
    }
}

public class UpdatePanelHandler(IUnitOfWork uow)
    : IRequestHandler<UpdatePanelCommand, bool>
{
    public async Task<bool> Handle(UpdatePanelCommand request, CancellationToken ct)
    {
        var entity = await uow.PanelMasters.GetByIdAsync(request.PanelId, ct);
        if (entity is null) return false;
        entity.UpdateName(request.PanelName);
        uow.PanelMasters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateAccessMasterHandler(IUnitOfWork uow)
    : IRequestHandler<CreateAccessMasterCommand, decimal>
{
    public async Task<decimal> Handle(CreateAccessMasterCommand request, CancellationToken ct)
    {
        var all = await uow.UnitLovAccessMasters.GetAllAsync(ct);
        var nextId = all.Any() ? all.Max(x => x.UaAccessMastId) + 1 : 1;
        var entity = UnitLovAccessMaster.Create(nextId, request.UnitLovMapId, request.DepartmentId, request.ProcessId);
        await uow.UnitLovAccessMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.UaAccessMastId;
    }
}
