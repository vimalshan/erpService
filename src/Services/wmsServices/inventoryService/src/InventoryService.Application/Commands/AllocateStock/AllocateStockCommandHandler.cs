using MediatR;
using InventoryService.Domain.Exceptions;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Commands.AllocateStock;

public class AllocateStockCommandHandler : IRequestHandler<AllocateStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AllocateStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AllocateStockCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await _unitOfWork.StockLevels
            .GetByProductAndBinAsync(request.ProductId, request.BinId, cancellationToken)
            ?? throw new InsufficientStockException(request.ProductId, request.WarehouseId, request.BinId, request.Quantity, 0);

        stockLevel.Allocate(request.Quantity);
        await _unitOfWork.StockLevels.UpdateAsync(stockLevel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
