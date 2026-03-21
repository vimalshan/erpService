using MediatR;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Racks.Commands.DeleteRack;

public sealed class DeleteRackCommandHandler : IRequestHandler<DeleteRackCommand>
{
    private readonly IUnitOfWork _uow;

    public DeleteRackCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteRackCommand request, CancellationToken ct)
    {
        var rack = await _uow.Racks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Rack), request.Id);

        rack.Deactivate();
        _uow.Racks.Update(rack);
        await _uow.SaveChangesAsync(ct);
    }
}
