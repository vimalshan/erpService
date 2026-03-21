using MediatR;
using UnitService.Application.DTOs;

namespace UnitService.Application.Queries.GetEquipmentStatus;

public record GetEquipmentStatusQuery(int EquipmentId) : IRequest<IEnumerable<EquipmentStatusDto>>;
