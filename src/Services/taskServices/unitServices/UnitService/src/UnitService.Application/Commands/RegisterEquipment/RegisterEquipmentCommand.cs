using MediatR;

namespace UnitService.Application.Commands.RegisterEquipment;

public record RegisterEquipmentCommand(
    int EquipmentId,
    string EquipmentName,
    string UnitCode,
    string Category,
    int ModifiedBy) : IRequest<int>;
