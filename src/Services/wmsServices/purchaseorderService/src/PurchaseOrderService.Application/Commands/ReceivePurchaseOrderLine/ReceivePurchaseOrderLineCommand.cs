using MediatR;

namespace PurchaseOrderService.Application.Commands.ReceivePurchaseOrderLine;

public record ReceivePurchaseOrderLineCommand : IRequest<Unit>
{
    public int PoId { get; init; }
    public int LineNumber { get; init; }
    public decimal Quantity { get; init; }
}
