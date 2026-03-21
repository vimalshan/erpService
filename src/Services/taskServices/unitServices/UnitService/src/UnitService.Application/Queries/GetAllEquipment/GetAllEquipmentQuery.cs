using MediatR;
using UnitService.Application.DTOs;

namespace UnitService.Application.Queries.GetAllEquipment;

public record GetAllEquipmentQuery : IRequest<IEnumerable<EquipmentDto>>;
