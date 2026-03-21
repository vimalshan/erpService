using MediatR;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.DeleteWarehouse;

public sealed class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, bool>
{
    private readonly IWarehouseRepository _repository;

    public DeleteWarehouseCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Warehouse with Id {request.Id} not found.");

        warehouse.RaiseDeletedEvent();
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
