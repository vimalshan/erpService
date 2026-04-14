using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Application.Queries.GetPurchaseOrder;
using WMTransactional.Application.Queries.GetPurchaseOrders;
using WMTransactional.Application.Queries.GetReceiving;
using WMTransactional.Application.Queries.GetReceivings;
using WMTransactional.Application.Queries.GetSalesOrder;
using WMTransactional.Application.Queries.GetSalesOrders;
using WMTransactional.Application.Queries.GetShipment;
using WMTransactional.Application.Queries.GetShipments;

namespace WMTransactional.API.GraphQL;

public class TransactionalQuery
{
    public async Task<PurchaseOrderDto?> GetPurchaseOrder(
        [Service] IMediator mediator,
        int purchaseOrderId)
    {
        return await mediator.Send(new GetPurchaseOrderQuery(purchaseOrderId));
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrders(
        [Service] IMediator mediator,
        string? status = null)
    {
        return await mediator.Send(new GetPurchaseOrdersQuery { Status = status });
    }

    public async Task<ReceivingDto?> GetReceiving(
        [Service] IMediator mediator,
        int receivingId)
    {
        return await mediator.Send(new GetReceivingQuery(receivingId));
    }

    public async Task<IEnumerable<ReceivingDto>> GetReceivings(
        [Service] IMediator mediator,
        int? purchaseOrderId = null)
    {
        return await mediator.Send(new GetReceivingsQuery { PoId = purchaseOrderId });
    }

    public async Task<SalesOrderDto?> GetSalesOrder(
        [Service] IMediator mediator,
        int salesOrderId)
    {
        return await mediator.Send(new GetSalesOrderQuery(salesOrderId));
    }

    public async Task<IEnumerable<SalesOrderDto>> GetSalesOrders(
        [Service] IMediator mediator,
        string? status = null)
    {
        return await mediator.Send(new GetSalesOrdersQuery { Status = status });
    }

    public async Task<ShipmentDto?> GetShipment(
        [Service] IMediator mediator,
        int shipmentId)
    {
        return await mediator.Send(new GetShipmentQuery(shipmentId));
    }

    public async Task<IEnumerable<ShipmentDto>> GetShipments(
        [Service] IMediator mediator,
        int? salesOrderId = null)
    {
        return await mediator.Send(new GetShipmentsQuery { SoId = salesOrderId });
    }
}
