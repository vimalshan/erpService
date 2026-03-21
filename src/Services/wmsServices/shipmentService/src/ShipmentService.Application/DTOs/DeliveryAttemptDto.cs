using ShipmentService.Domain.Entities;

namespace ShipmentService.Application.DTOs;

public sealed record DeliveryAttemptDto(
    int AttemptId,
    int ShipmentId,
    DateTime AttemptDate,
    string Result,
    string? Reason,
    string? Notes)
{
    public static DeliveryAttemptDto FromEntity(DeliveryAttempt d) => new(
        d.Id, d.ShipmentId, d.AttemptDate, d.Result.ToString(), d.Reason, d.Notes);
}
