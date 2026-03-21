using MediatR;

namespace PurchaseOrderService.Application.Commands.CreatePurchaseOrder;

public record CreatePurchaseOrderCommand : IRequest<int>
{
    public string PoNumber { get; init; } = null!;
    public int SupplierId { get; init; }
    public int WarehouseId { get; init; }
    public DateTime OrderDate { get; init; }
    public DateTime? ExpectedDate { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public List<CreatePurchaseOrderLineCommand> Lines { get; init; } = new();
}

public record CreatePurchaseOrderLineCommand
{
    public int ProductId { get; init; }
    public int LineNumber { get; init; }
    public decimal QuantityOrdered { get; init; }
    public decimal? UnitPrice { get; init; }
    public string? Notes { get; init; }
}
