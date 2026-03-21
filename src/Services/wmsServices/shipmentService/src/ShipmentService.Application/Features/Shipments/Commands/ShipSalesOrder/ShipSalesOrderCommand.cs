using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Commands.ShipSalesOrder;

public sealed record ShipSalesOrderItem(
    int SoLineId,
    int ProductId,
    int BinId,
    decimal Quantity,
    string? LotNumber);

public sealed record ShipSalesOrderCommand(
    string ShipmentNumber,
    int SoId,
    IEnumerable<ShipSalesOrderItem> Items,
    string? Carrier,
    string? TrackingNumber,
    string? CreatedBy) : IRequest<ShipmentDto>;
