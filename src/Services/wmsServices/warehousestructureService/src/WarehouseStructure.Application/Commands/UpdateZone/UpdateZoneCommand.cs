using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Commands.UpdateZone;

public sealed record UpdateZoneCommand(int Id, UpdateZoneDto Dto) : IRequest<ZoneDto>;
