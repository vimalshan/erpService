using MediatR;
using LovService.Application.DTOs;
using LovService.Domain.Interfaces;

namespace LovService.Application.Features.ProgramLovMast.Commands;

public sealed class CreateProgramLovCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CreateProgramLovCommand, ProgramLovMastDto>
{
    public async Task<ProgramLovMastDto> Handle(CreateProgramLovCommand cmd, CancellationToken ct)
    {
        var entity = Domain.Entities.ProgramLovMast.Create(cmd.PrlovTypeCode, cmd.PrlovCode, cmd.PrlovName);
        await uow.ProgramLovMasts.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return new ProgramLovMastDto(entity.PrlovTypeCode, entity.PrlovCode, entity.PrlovName);
    }
}

public sealed class UpdateProgramLovCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateProgramLovCommand, ProgramLovMastDto>
{
    public async Task<ProgramLovMastDto> Handle(UpdateProgramLovCommand cmd, CancellationToken ct)
    {
        var entity = await uow.ProgramLovMasts.GetByIdAsync(cmd.PrlovTypeCode, cmd.PrlovCode, ct)
            ?? throw new KeyNotFoundException($"ProgramLov {cmd.PrlovTypeCode}/{cmd.PrlovCode} not found.");

        entity.Update(cmd.PrlovName);
        await uow.ProgramLovMasts.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return new ProgramLovMastDto(entity.PrlovTypeCode, entity.PrlovCode, entity.PrlovName);
    }
}

public sealed class DeleteProgramLovCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteProgramLovCommand, bool>
{
    public async Task<bool> Handle(DeleteProgramLovCommand cmd, CancellationToken ct)
    {
        if (!await uow.ProgramLovMasts.ExistsAsync(cmd.PrlovTypeCode, cmd.PrlovCode, ct))
            return false;

        await uow.ProgramLovMasts.DeleteAsync(cmd.PrlovTypeCode, cmd.PrlovCode, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
