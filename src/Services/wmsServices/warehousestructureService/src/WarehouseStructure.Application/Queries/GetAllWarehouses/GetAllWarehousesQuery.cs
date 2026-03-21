using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Queries.GetAllWarehouses;

public sealed record GetAllWarehousesQuery : IRequest<IEnumerable<WarehouseDto>>;
