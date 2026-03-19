using MediatR;
using StrategicStock.Domain.Entities;
using StrategicStock.Domain.Interfaces;

namespace StrategicStock.Application.Commands.CreateStrategicStock;

public sealed class CreateStrategicStockCommandHandler(IStrategicStockRepository repository)
    : IRequestHandler<CreateStrategicStockCommand, int>
{
    public async Task<int> Handle(CreateStrategicStockCommand request, CancellationToken cancellationToken)
    {
        var entity = StrategicStockEntity.Create(
            request.StrategicStockId,
            request.SciItemId,
            request.CompanyUnitId,
            request.StockTypeCode,
            request.MaxQty,
            request.EffectiveDate,
            request.CreatedByUserId);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
