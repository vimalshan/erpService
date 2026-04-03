using MediatR;
using LovService.Application.DTOs;
using LovService.Domain.Interfaces;

namespace LovService.Application.Features.LovTypeMast.Commands;

public sealed class CreateLovTypeCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CreateLovTypeCommand, LovTypeMastDto>
{
    public async Task<LovTypeMastDto> Handle(CreateLovTypeCommand cmd, CancellationToken ct)
    {
        var entity = Domain.Entities.LovTypeMast.Create(
            cmd.LovTypeId, cmd.LovTypeName, cmd.LovCategory[0], cmd.LovOrgId);

        await uow.LovTypeMasts.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        return new LovTypeMastDto(
            entity.LovTypeId, entity.LovTypeName, entity.LovCategory.Value.ToString(), entity.LovOrgId);
    }
}

public sealed class UpdateLovTypeCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateLovTypeCommand, LovTypeMastDto>
{
    public async Task<LovTypeMastDto> Handle(UpdateLovTypeCommand cmd, CancellationToken ct)
    {
        var entity = await uow.LovTypeMasts.GetByIdAsync(cmd.LovTypeId, ct)
            ?? throw new KeyNotFoundException($"LovType {cmd.LovTypeId} not found.");

        entity.Update(cmd.LovTypeName, cmd.LovCategory[0], cmd.LovOrgId);
        await uow.LovTypeMasts.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        return new LovTypeMastDto(
            entity.LovTypeId, entity.LovTypeName, entity.LovCategory.Value.ToString(), entity.LovOrgId);
    }
}

public sealed class DeleteLovTypeCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteLovTypeCommand, bool>
{
    public async Task<bool> Handle(DeleteLovTypeCommand cmd, CancellationToken ct)
    {
        if (!await uow.LovTypeMasts.ExistsAsync(cmd.LovTypeId, ct))
            return false;

        await uow.LovTypeMasts.DeleteAsync(cmd.LovTypeId, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
