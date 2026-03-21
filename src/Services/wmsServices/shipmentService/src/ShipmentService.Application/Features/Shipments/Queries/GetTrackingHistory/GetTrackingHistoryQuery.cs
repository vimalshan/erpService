using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Queries.GetTrackingHistory;

public sealed record GetTrackingHistoryQuery(int ShipmentId) : IRequest<IEnumerable<TrackingHistoryDto>>;
