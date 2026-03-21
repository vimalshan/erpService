using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Commands.AdjustInventory;

public record AdjustInventoryCommand : IRequest<StockLevelDto>
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int BinId { get; init; }
    public decimal NewQuantity { get; init; }
    public string Reason { get; init; } = null!;
    public string AdjustedBy { get; init; } = null!;
}
