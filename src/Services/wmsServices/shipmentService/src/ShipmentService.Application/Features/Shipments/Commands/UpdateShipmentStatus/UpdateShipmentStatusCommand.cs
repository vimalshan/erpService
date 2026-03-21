using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Commands.UpdateShipmentStatus;

public sealed record UpdateShipmentStatusCommand(
    int ShipmentId,
    string NewStatus,
    string? Location,
    string? Description,
    string? UpdatedBy) : IRequest<ShipmentDto>;
