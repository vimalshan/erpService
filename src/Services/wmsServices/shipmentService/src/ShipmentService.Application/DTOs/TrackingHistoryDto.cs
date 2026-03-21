using ShipmentService.Domain.Entities;

namespace ShipmentService.Application.DTOs;

public sealed record TrackingHistoryDto(
    int TrackingId,
    int ShipmentId,
    string Status,
    string? Location,
    string? Description,
    DateTime EventDatetime,
    string? CreatedBy)
{
    public static TrackingHistoryDto FromEntity(TrackingHistory t) => new(
        t.Id, t.ShipmentId, t.Status, t.Location, t.Description, t.EventDatetime, t.CreatedBy);
}
