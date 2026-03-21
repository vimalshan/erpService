using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;

public sealed record GetAllShipmentsQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<ShipmentSummaryDto>>;

public sealed record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
