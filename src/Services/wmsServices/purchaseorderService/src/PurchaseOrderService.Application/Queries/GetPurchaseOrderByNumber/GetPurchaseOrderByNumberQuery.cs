using MediatR;
using PurchaseOrderService.Application.DTOs;

namespace PurchaseOrderService.Application.Queries.GetPurchaseOrderByNumber;

public record GetPurchaseOrderByNumberQuery(string PoNumber) : IRequest<PurchaseOrderDto?>;
