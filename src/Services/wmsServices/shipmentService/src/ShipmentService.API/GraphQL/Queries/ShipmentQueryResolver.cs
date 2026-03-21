using ShipmentService.Application.DTOs;
using ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;
using ShipmentService.Application.Features.Shipments.Queries.GetShipmentById;
using ShipmentService.Application.Features.Shipments.Queries.GetShipmentsByCustomer;
using ShipmentService.Application.Features.Shipments.Queries.GetTrackingHistory;
using ShipmentService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ShipmentService.API.GraphQL.Queries;

public sealed class ShipmentQueryResolver
{
    public async Task<ShipmentDto> GetShipmentByIdAsync(
        int id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetShipmentByIdQuery(id), ct);

    public async Task<PagedResult<ShipmentSummaryDto>> GetShipmentsAsync(
        int page, int pageSize, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllShipmentsQuery(page, pageSize), ct);

    public async Task<IEnumerable<ShipmentSummaryDto>> GetShipmentsByCustomerAsync(
        int customerId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetShipmentsByCustomerQuery(customerId), ct);

    public async Task<IEnumerable<TrackingHistoryDto>> GetTrackingHistoryAsync(
        int shipmentId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetTrackingHistoryQuery(shipmentId), ct);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.Shipment> GetShipments([Service] ShipmentDbContext context)
        => context.Shipments.Include(s => s.Lines).Include(s => s.Packages);
}
