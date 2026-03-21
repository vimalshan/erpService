using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Commands.ReceiveStock;

public record ReceiveStockCommand : IRequest<StockLevelDto>
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int BinId { get; init; }
    public decimal Quantity { get; init; }
    public string? ReferenceNumber { get; init; }
    public string? CreatedBy { get; init; }
}
