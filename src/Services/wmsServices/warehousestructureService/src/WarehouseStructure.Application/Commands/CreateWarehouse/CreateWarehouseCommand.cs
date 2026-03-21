using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Commands.CreateWarehouse;

public sealed record CreateWarehouseCommand(CreateWarehouseDto Dto) : IRequest<WarehouseDto>;
