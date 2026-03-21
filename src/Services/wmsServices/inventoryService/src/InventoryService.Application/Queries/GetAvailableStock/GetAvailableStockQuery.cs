using MediatR;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Queries.GetAvailableStock;

public record GetAvailableStockQuery(int ProductId, int? WarehouseId = null, int? BinId = null) : IRequest<decimal>;

public class GetAvailableStockQueryHandler : IRequestHandler<GetAvailableStockQuery, decimal>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAvailableStockQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> Handle(GetAvailableStockQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.StockLevels.GetAvailableStockAsync(
            request.ProductId, request.WarehouseId, request.BinId, cancellationToken);
    }
}
