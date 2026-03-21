using MediatR;

namespace UnitService.Application.Commands.UpdateEquipmentStatus;

public record UpdateEquipmentStatusCommand(
    int StatusId,
    int EquipmentId,
    string StatusDescription,
    string StatusCode,
    string? Remarks,
    long? Hours,
    int CreatedBy) : IRequest<int>;
