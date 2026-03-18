using MediatR;
using LovService.Application.DTOs;
using LovService.Domain.Interfaces;

namespace LovService.Application.Features.LovMaster.Commands;

public sealed class CreateLovMasterCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CreateLovMasterCommand, LovMasterDto>
{
    public async Task<LovMasterDto> Handle(CreateLovMasterCommand cmd, CancellationToken ct)
    {
        var nextId = await uow.LovMasters.GetNextIdAsync(ct);
        var entity = Domain.Entities.LovMaster.Create(nextId, cmd.LovTypeId, cmd.LovName, cmd.CreatedBy);

        await uow.LovMasters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        return ToDto(entity);
    }

    private static LovMasterDto ToDto(Domain.Entities.LovMaster e)
        => new(e.LovId, e.LovTypeId, e.LovName, e.LovCreatedOn, e.LovCreatedBy, e.LovUpdatedBy, e.LovUpdatedOn);
}

public sealed class UpdateLovMasterCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateLovMasterCommand, LovMasterDto>
{
    public async Task<LovMasterDto> Handle(UpdateLovMasterCommand cmd, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(cmd.LovId, ct)
            ?? throw new KeyNotFoundException($"LovMaster {cmd.LovId} not found.");

        entity.Update(cmd.LovName, cmd.UpdatedBy);
        await uow.LovMasters.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        return new LovMasterDto(entity.LovId, entity.LovTypeId, entity.LovName,
            entity.LovCreatedOn, entity.LovCreatedBy, entity.LovUpdatedBy, entity.LovUpdatedOn);
    }
}

public sealed class DeleteLovMasterCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteLovMasterCommand, bool>
{
    public async Task<bool> Handle(DeleteLovMasterCommand cmd, CancellationToken ct)
    {
        var entity = await uow.LovMasters.GetByIdAsync(cmd.LovId, ct);
        if (entity == null) return false;

        entity.Delete();
        await uow.LovMasters.DeleteAsync(cmd.LovId, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
