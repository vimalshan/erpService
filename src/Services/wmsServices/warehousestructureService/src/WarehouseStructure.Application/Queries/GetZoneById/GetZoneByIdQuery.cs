using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Queries.GetZoneById;

public sealed record GetZoneByIdQuery(int Id) : IRequest<ZoneDto?>;
