using MediatR;
using PurchaseOrderService.Application.DTOs;

namespace PurchaseOrderService.Application.Queries.GetPurchaseOrderById;

public record GetPurchaseOrderByIdQuery(int PoId) : IRequest<PurchaseOrderDto?>;
