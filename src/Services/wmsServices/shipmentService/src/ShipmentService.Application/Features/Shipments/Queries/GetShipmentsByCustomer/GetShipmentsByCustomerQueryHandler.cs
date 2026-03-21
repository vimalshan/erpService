using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Queries.GetShipmentsByCustomer;

public sealed class GetShipmentsByCustomerQueryHandler : IRequestHandler<GetShipmentsByCustomerQuery, IEnumerable<ShipmentSummaryDto>>
{
    private readonly IShipmentRepository _repository;

    public GetShipmentsByCustomerQueryHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<IEnumerable<ShipmentSummaryDto>> Handle(GetShipmentsByCustomerQuery request, CancellationToken cancellationToken)
    {
        var shipments = await _repository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        return shipments.Select(ShipmentSummaryDto.FromEntity);
    }
}
