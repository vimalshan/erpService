using MediatR;

namespace PurchaseOrderService.Application.Commands.ConfirmPurchaseOrder;

public record ConfirmPurchaseOrderCommand(int PoId) : IRequest<Unit>;
