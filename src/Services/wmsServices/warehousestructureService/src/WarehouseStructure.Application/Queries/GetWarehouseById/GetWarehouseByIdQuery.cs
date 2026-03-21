using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Queries.GetWarehouseById;

public sealed record GetWarehouseByIdQuery(int Id) : IRequest<WarehouseDto?>;
