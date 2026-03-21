using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Queries.GetShipmentById;

public sealed record GetShipmentByIdQuery(int ShipmentId) : IRequest<ShipmentDto>;
