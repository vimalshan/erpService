using MediatR;
using StrategicStock.Domain.Interfaces;

namespace StrategicStock.Application.Commands.UpdateStrategicStock;

public sealed class UpdateStrategicStockCommandHandler(IStrategicStockRepository repository)
    : IRequestHandler<UpdateStrategicStockCommand>
{
    public async Task Handle(UpdateStrategicStockCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.StrategicStockId, cancellationToken)
            ?? throw new KeyNotFoundException($"Strategic stock {request.StrategicStockId} not found.");

        entity.Update(request.MaxQty, request.FilledQty, request.StockTypeCode, request.ModifiedByUserId);
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
