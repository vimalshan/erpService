using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Queries.GetAllZones;

public sealed record GetAllZonesQuery(int? WarehouseId = null) : IRequest<IEnumerable<ZoneDto>>;
