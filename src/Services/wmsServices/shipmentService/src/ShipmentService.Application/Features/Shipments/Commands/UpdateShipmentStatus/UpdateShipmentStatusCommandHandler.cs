using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Enums;
using ShipmentService.Domain.Exceptions;

namespace ShipmentService.Application.Features.Shipments.Commands.UpdateShipmentStatus;

public sealed class UpdateShipmentStatusCommandHandler : IRequestHandler<UpdateShipmentStatusCommand, ShipmentDto>
{
    private readonly IShipmentRepository _repository;
    private readonly IMessagePublisher _publisher;

    public UpdateShipmentStatusCommandHandler(IShipmentRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<ShipmentDto> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new ShipmentNotFoundException(request.ShipmentId);

        var newStatus = Enum.Parse<ShipmentStatus>(
            request.NewStatus.Replace("_", ""), ignoreCase: true);

        shipment.UpdateStatus(newStatus, request.Location, request.Description, request.UpdatedBy);

        await _repository.UpdateAsync(shipment, cancellationToken);

        await _publisher.PublishAsync(
            "shipment.exchange",
            "shipment.status_changed",
            new { shipment.ShipmentNumber, NewStatus = newStatus.ToString(), request.Location, request.UpdatedBy },
            cancellationToken);

        return ShipmentDto.FromEntity(shipment);
    }
}
