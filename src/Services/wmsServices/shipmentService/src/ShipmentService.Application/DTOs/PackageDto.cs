using ShipmentService.Domain.Entities;

namespace ShipmentService.Application.DTOs;

public sealed record PackageDto(
    int PackageId,
    int ShipmentId,
    string PackageNumber,
    decimal? Weight,
    decimal? Volume,
    string? Dimensions,
    string? TrackingNumber,
    string? ContentsDescription)
{
    public static PackageDto FromEntity(Package p) => new(
        p.Id, p.ShipmentId, p.PackageNumber, p.Weight, p.Volume,
        p.Dimensions, p.TrackingNumber, p.ContentsDescription);
}
