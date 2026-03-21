using ArchiveService.Domain.Entities;
using ArchiveService.Domain.Interfaces;
using MediatR;

namespace ArchiveService.Application.Features.ToolKits.Commands;

public class CreateToolKitHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateToolKitCommand, long>
{
    public async Task<long> Handle(CreateToolKitCommand cmd, CancellationToken ct)
    {
        var toolkit = ArchivedToolKit.Create(
            cmd.KitCode, cmd.AppPassword, cmd.InstPassword,
            cmd.ImeiNo, cmd.EngineerId, cmd.Flag, cmd.EnteredBy);

        await unitOfWork.ToolKits.AddAsync(toolkit, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return toolkit.Id;
    }
}

public class UpdateToolKitFlagHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateToolKitFlagCommand, bool>
{
    public async Task<bool> Handle(UpdateToolKitFlagCommand cmd, CancellationToken ct)
    {
        var toolkit = await unitOfWork.ToolKits.GetByIdAsync(cmd.Id, ct);
        if (toolkit is null) return false;

        toolkit.UpdateFlag(cmd.Flag, cmd.ChangedBy);
        await unitOfWork.ToolKits.UpdateAsync(toolkit, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
