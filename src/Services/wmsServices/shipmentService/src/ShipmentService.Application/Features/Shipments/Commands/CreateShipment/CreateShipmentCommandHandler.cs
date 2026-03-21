using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Entities;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, ShipmentDto>
{
    private readonly IShipmentRepository _repository;
    private readonly IMessagePublisher _publisher;

    public CreateShipmentCommandHandler(IShipmentRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<ShipmentDto> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.ShipmentNumber, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Shipment number '{request.ShipmentNumber}' already exists.");

        var shipmentType = Enum.Parse<ShipmentType>(request.ShipmentType, ignoreCase: true);

        var shipment = Shipment.Create(
            request.ShipmentNumber,
            request.CustomerId,
            request.WarehouseId,
            shipmentType,
            request.ServiceType,
            request.Carrier,
            request.TrackingNumber,
            request.SpecialInstructions,
            request.CreatedBy,
            request.SoId);

        await _repository.AddAsync(shipment, cancellationToken);

        await _publisher.PublishAsync(
            "shipment.exchange",
            "shipment.created",
            new { shipment.ShipmentNumber, shipment.CustomerId, shipment.WarehouseId, CreatedAt = DateTime.UtcNow },
            cancellationToken);

        return ShipmentDto.FromEntity(shipment);
    }
}
