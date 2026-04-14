using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.ConfirmPurchaseOrder;

public record ConfirmPurchaseOrderCommand(int PoId) : IRequest<PurchaseOrderDto>;
