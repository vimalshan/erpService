using ShipmentService.Domain.Entities;

namespace ShipmentService.Application.DTOs;

public sealed record ShipmentDto(
    int ShipmentId,
    string ShipmentNumber,
    int? SoId,
    int CustomerId,
    int WarehouseId,
    string ShipmentType,
    string? ServiceType,
    DateTime ShippedDate,
    string Status,
    string? TrackingNumber,
    string? Carrier,
    decimal? TotalWeight,
    decimal? TotalVolume,
    string? SpecialInstructions,
    string? Notes,
    string? CreatedBy,
    DateTime CreatedDate,
    DateTime ModifiedDate,
    IReadOnlyCollection<ShipmentLineDto> Lines,
    IReadOnlyCollection<PackageDto> Packages,
    IReadOnlyCollection<TrackingHistoryDto> TrackingHistory,
    IReadOnlyCollection<DeliveryAttemptDto> DeliveryAttempts)
{
    public static ShipmentDto FromEntity(Shipment s) => new(
        s.Id, s.ShipmentNumber, s.SoId, s.CustomerId, s.WarehouseId,
        s.ShipmentType.ToString(), s.ServiceType, s.ShippedDate, s.Status.ToString(),
        s.TrackingNumber, s.Carrier, s.TotalWeight, s.TotalVolume,
        s.SpecialInstructions, s.Notes, s.CreatedBy, s.CreatedDate, s.ModifiedDate,
        s.Lines.Select(ShipmentLineDto.FromEntity).ToList().AsReadOnly(),
        s.Packages.Select(PackageDto.FromEntity).ToList().AsReadOnly(),
        s.TrackingHistory.Select(TrackingHistoryDto.FromEntity).ToList().AsReadOnly(),
        s.DeliveryAttempts.Select(DeliveryAttemptDto.FromEntity).ToList().AsReadOnly());
}

public sealed record ShipmentSummaryDto(
    int ShipmentId,
    string ShipmentNumber,
    int CustomerId,
    int WarehouseId,
    string ShipmentType,
    string? ServiceType,
    string Status,
    string? TrackingNumber,
    string? Carrier,
    DateTime ShippedDate,
    DateTime CreatedDate)
{
    public static ShipmentSummaryDto FromEntity(Shipment s) => new(
        s.Id, s.ShipmentNumber, s.CustomerId, s.WarehouseId, s.ShipmentType.ToString(),
        s.ServiceType, s.Status.ToString(), s.TrackingNumber, s.Carrier, s.ShippedDate, s.CreatedDate);
}
