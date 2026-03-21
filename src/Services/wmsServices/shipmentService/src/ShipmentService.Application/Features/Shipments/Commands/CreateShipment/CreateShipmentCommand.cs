using MediatR;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Application.Features.Shipments.Commands.CreateShipment;

public sealed record CreateShipmentCommand(
    string ShipmentNumber,
    int CustomerId,
    int WarehouseId,
    string ShipmentType,
    string? ServiceType,
    string? Carrier,
    string? TrackingNumber,
    string? SpecialInstructions,
    string? Notes,
    int? SoId,
    string? CreatedBy) : IRequest<ShipmentDto>;
