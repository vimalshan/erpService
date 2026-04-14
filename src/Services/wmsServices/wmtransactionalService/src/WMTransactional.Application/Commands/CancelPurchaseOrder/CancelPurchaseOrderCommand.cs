using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CancelPurchaseOrder;

public record CancelPurchaseOrderCommand(int PoId) : IRequest<PurchaseOrderDto>;
