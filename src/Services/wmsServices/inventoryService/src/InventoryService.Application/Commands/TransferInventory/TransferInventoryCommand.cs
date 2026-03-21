using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Commands.TransferInventory;

public record TransferInventoryCommand : IRequest<Unit>
{
    public int ProductId { get; init; }
    public int FromWarehouseId { get; init; }
    public int FromBinId { get; init; }
    public int ToWarehouseId { get; init; }
    public int ToBinId { get; init; }
    public decimal Quantity { get; init; }
    public string? ReferenceNumber { get; init; }
    public string? CreatedBy { get; init; }
}
