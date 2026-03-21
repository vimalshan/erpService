using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Entities;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Application.Features.Shipments.Commands.ShipSalesOrder;

public sealed class ShipSalesOrderCommandHandler : IRequestHandler<ShipSalesOrderCommand, ShipmentDto>
{
    private readonly IShipmentRepository _repository;
    private readonly IMessagePublisher _publisher;

    public ShipSalesOrderCommandHandler(IShipmentRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<ShipmentDto> Handle(ShipSalesOrderCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsAsync(request.ShipmentNumber, cancellationToken))
            throw new InvalidOperationException($"Shipment '{request.ShipmentNumber}' already exists.");

        // In production, customer_id and warehouse_id would be fetched from SalesOrder service.
        // Here we use placeholder values that will be resolved at the infrastructure layer.
        var shipment = Shipment.Create(
            request.ShipmentNumber,
            customerId: 0,       // resolved by Infrastructure via sp_ShipSalesOrder
            warehouseId: 0,
            ShipmentType.Outbound,
            carrier: request.Carrier,
            trackingNumber: request.TrackingNumber,
            createdBy: request.CreatedBy,
            soId: request.SoId);

        foreach (var item in request.Items)
        {
            shipment.AddLine(
                item.ProductId,
                item.BinId,
                item.Quantity,
                lotNumber: item.LotNumber,
                soLineId: item.SoLineId);
        }

        await _repository.AddAsync(shipment, cancellationToken);

        await _publisher.PublishAsync(
            "shipment.exchange",
            "shipment.sales_order_shipped",
            new { request.ShipmentNumber, request.SoId, CreatedAt = DateTime.UtcNow },
            cancellationToken);

        return ShipmentDto.FromEntity(shipment);
    }
}
