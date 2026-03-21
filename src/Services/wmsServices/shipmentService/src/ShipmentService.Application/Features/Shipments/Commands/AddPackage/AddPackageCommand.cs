using MediatR;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Application.Features.Shipments.Commands.AddPackage;

public sealed record AddPackageCommand(
    int ShipmentId,
    string PackageNumber,
    decimal? Weight,
    decimal? Volume,
    string? Dimensions,
    string? TrackingNumber,
    string? ContentsDescription) : IRequest<PackageDto>;
