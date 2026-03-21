using MediatR;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.DTOs;
using ShipmentService.Domain.Exceptions;

namespace ShipmentService.Application.Features.Shipments.Commands.AddPackage;

public sealed class AddPackageCommandHandler : IRequestHandler<AddPackageCommand, PackageDto>
{
    private readonly IShipmentRepository _repository;

    public AddPackageCommandHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<PackageDto> Handle(AddPackageCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new ShipmentNotFoundException(request.ShipmentId);

        var package = shipment.AddPackage(
            request.PackageNumber,
            request.Weight,
            request.Volume,
            request.Dimensions,
            request.TrackingNumber,
            request.ContentsDescription);

        await _repository.UpdateAsync(shipment, cancellationToken);
        return PackageDto.FromEntity(package);
    }
}
