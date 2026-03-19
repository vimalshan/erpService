using MediatR;
using StrategicStock.Domain.Interfaces;

namespace StrategicStock.Application.Commands.CloseStrategicStock;

public sealed class CloseStrategicStockCommandHandler(IStrategicStockRepository repository)
    : IRequestHandler<CloseStrategicStockCommand>
{
    public async Task Handle(CloseStrategicStockCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.StrategicStockId, cancellationToken)
            ?? throw new KeyNotFoundException($"Strategic stock {request.StrategicStockId} not found.");

        entity.Close(request.ModifiedByUserId);
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
