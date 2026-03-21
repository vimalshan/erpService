using MediatR;

namespace PurchaseOrderService.Application.Commands.CancelPurchaseOrder;

public record CancelPurchaseOrderCommand(int PoId) : IRequest<Unit>;
