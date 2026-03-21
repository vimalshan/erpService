using MediatR;

namespace PurchaseOrderService.Application.Commands.UpdatePurchaseOrder;

public record UpdatePurchaseOrderCommand : IRequest<Unit>
{
    public int PoId { get; init; }
    public DateTime? ExpectedDate { get; init; }
    public string? Notes { get; init; }
    public List<UpdatePurchaseOrderLineCommand> Lines { get; init; } = new();
}

public record UpdatePurchaseOrderLineCommand
{
    public int ProductId { get; init; }
    public int LineNumber { get; init; }
    public decimal QuantityOrdered { get; init; }
    public decimal? UnitPrice { get; init; }
    public string? Notes { get; init; }
}
