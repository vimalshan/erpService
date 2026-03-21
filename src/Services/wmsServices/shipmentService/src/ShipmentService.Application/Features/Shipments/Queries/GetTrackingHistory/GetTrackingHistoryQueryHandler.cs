using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Exceptions;

namespace ShipmentService.Application.Features.Shipments.Queries.GetTrackingHistory;

public sealed class GetTrackingHistoryQueryHandler : IRequestHandler<GetTrackingHistoryQuery, IEnumerable<TrackingHistoryDto>>
{
    private readonly IShipmentRepository _repository;

    public GetTrackingHistoryQueryHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<IEnumerable<TrackingHistoryDto>> Handle(GetTrackingHistoryQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new ShipmentNotFoundException(request.ShipmentId);
        return shipment.TrackingHistory.Select(TrackingHistoryDto.FromEntity);
    }
}
