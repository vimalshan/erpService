using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Commands.CreateZone;

public sealed record CreateZoneCommand(CreateZoneDto Dto) : IRequest<ZoneDto>;
