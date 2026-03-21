using MediatR;
using PurchaseOrderService.Application.DTOs;
using PurchaseOrderService.Application.Queries.GetPurchaseOrderById;
using PurchaseOrderService.Application.Queries.GetPurchaseOrderByNumber;
using PurchaseOrderService.Application.Queries.GetPurchaseOrders;

namespace PurchaseOrderService.API.GraphQL;

public class PurchaseOrderQuery
{
    public async Task<PurchaseOrderDto?> GetPurchaseOrder(int id, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetPurchaseOrderByIdQuery(id));
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderByNumber(string poNumber, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetPurchaseOrderByNumberQuery(poNumber));
    }

    public async Task<PurchaseOrdersResponse> GetPurchaseOrders(int page, int pageSize, string? status, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetPurchaseOrdersQuery { Page = page, PageSize = pageSize, Status = status });
    }
}
