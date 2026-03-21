using MediatR;

namespace InventoryService.Application.Commands.AllocateStock;

public record AllocateStockCommand : IRequest<Unit>
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int BinId { get; init; }
    public decimal Quantity { get; init; }
}
