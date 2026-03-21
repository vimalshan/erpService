using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Queries.GetShipmentsByCustomer;

public sealed record GetShipmentsByCustomerQuery(int CustomerId) : IRequest<IEnumerable<ShipmentSummaryDto>>;
