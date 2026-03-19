using IntegrationService.Application.DTOs;
using MediatR;

namespace IntegrationService.Application.PurchaseOrders.Queries;

public record GetPurchaseOrderByIdQuery(long PoSeqId) : IRequest<PurchaseOrderDto?>;
public record GetAllPurchaseOrdersQuery : IRequest<IEnumerable<PurchaseOrderDto>>;
public record GetPurchaseOrderWithMrcQuery(long PoSeqId) : IRequest<PurchaseOrderDto?>;
