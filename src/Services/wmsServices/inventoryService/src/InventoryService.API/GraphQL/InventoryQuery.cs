using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries.GetStockLevel;
using InventoryService.Application.Queries.GetInventoryByWarehouse;
using InventoryService.Application.Queries.GetAvailableStock;
using InventoryService.Application.Queries.GetTransactionHistory;
using InventoryService.Application.Queries.GetLowStockItems;

namespace InventoryService.API.GraphQL;

public class InventoryQuery
{
    public async Task<StockLevelDto?> GetStockLevel(
        [Service] IMediator mediator,
        long stockLevelId)
    {
        return await mediator.Send(new GetStockLevelQuery(stockLevelId));
    }

    public async Task<IEnumerable<StockLevelDto>> GetInventoryByWarehouse(
        [Service] IMediator mediator,
        int warehouseId)
    {
        return await mediator.Send(new GetInventoryByWarehouseQuery(warehouseId));
    }

    public async Task<decimal> GetAvailableStock(
        [Service] IMediator mediator,
        int productId,
        int? warehouseId = null,
        int? binId = null)
    {
        return await mediator.Send(new GetAvailableStockQuery(productId, warehouseId, binId));
    }

    public async Task<IEnumerable<StockLevelDto>> GetLowStockItems(
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetLowStockItemsQuery());
    }

    public async Task<IEnumerable<InventoryTransactionDto>> GetTransactionHistory(
        [Service] IMediator mediator,
        int? productId = null,
        int? warehouseId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        return await mediator.Send(new GetTransactionHistoryQuery
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            FromDate = fromDate,
            ToDate = toDate
        });
    }
}
