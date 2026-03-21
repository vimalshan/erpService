using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Exceptions;

namespace ShipmentService.Application.Features.Shipments.Queries.GetShipmentById;

public sealed class GetShipmentByIdQueryHandler : IRequestHandler<GetShipmentByIdQuery, ShipmentDto>
{
    private readonly IShipmentRepository _repository;

    public GetShipmentByIdQueryHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<ShipmentDto> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new ShipmentNotFoundException(request.ShipmentId);
        return ShipmentDto.FromEntity(shipment);
    }
}
