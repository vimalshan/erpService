using MediatR;
using UnitService.Application.DTOs;

namespace UnitService.Application.Queries.GetEquipment;

public record GetEquipmentQuery(int EquipmentId) : IRequest<EquipmentDto?>;
