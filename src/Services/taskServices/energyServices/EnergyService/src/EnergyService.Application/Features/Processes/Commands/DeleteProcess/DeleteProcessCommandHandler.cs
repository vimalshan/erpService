using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Processes.Commands.DeleteProcess;

public class DeleteProcessCommandHandler : IRequestHandler<DeleteProcessCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteProcessCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> Handle(DeleteProcessCommand request, CancellationToken ct)
    {
        var entity = await _uow.Processes.GetByIdAsync(request.EcProcessId, ct);
        if (entity is null) return false;

        _uow.Processes.Delete(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
