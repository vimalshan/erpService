using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;

namespace ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;

public sealed class GetAllShipmentsQueryHandler : IRequestHandler<GetAllShipmentsQuery, PagedResult<ShipmentSummaryDto>>
{
    private readonly IShipmentRepository _repository;

    public GetAllShipmentsQueryHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<PagedResult<ShipmentSummaryDto>> Handle(GetAllShipmentsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var shipments = await _repository.GetAllAsync(page, pageSize, cancellationToken);
        var total = await _repository.GetTotalCountAsync(cancellationToken);

        return new PagedResult<ShipmentSummaryDto>(
            shipments.Select(ShipmentSummaryDto.FromEntity),
            total, page, pageSize);
    }
}
