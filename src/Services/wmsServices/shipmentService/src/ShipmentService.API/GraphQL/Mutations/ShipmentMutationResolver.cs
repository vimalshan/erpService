using MediatR;
using ShipmentService.Application.DTOs;
using ShipmentService.Application.Features.Shipments.Commands.CreateShipment;
using ShipmentService.Application.Features.Shipments.Commands.UpdateShipmentStatus;
using ShipmentService.Application.Features.Shipments.Commands.AddPackage;

namespace ShipmentService.API.GraphQL.Mutations;

public sealed class ShipmentMutationResolver
{
    public async Task<ShipmentDto> CreateShipmentAsync(
        CreateShipmentCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<ShipmentDto> UpdateShipmentStatusAsync(
        int shipmentId, string newStatus, string? location, string? description, string? updatedBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateShipmentStatusCommand(shipmentId, newStatus, location, description, updatedBy), ct);

    public async Task<PackageDto> AddPackageAsync(
        AddPackageCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);
}
