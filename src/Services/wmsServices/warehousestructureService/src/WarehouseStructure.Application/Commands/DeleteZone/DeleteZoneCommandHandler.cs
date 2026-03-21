using MediatR;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.DeleteZone;

public sealed class DeleteZoneCommandHandler : IRequestHandler<DeleteZoneCommand, bool>
{
    private readonly IZoneRepository _repository;

    public DeleteZoneCommandHandler(IZoneRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteZoneCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.Id, cancellationToken))
            throw new KeyNotFoundException($"Zone with Id {request.Id} not found.");

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
