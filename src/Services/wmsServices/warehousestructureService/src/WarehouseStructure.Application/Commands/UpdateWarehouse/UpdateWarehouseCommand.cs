using MediatR;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Application.Commands.UpdateWarehouse;

public sealed record UpdateWarehouseCommand(int Id, UpdateWarehouseDto Dto) : IRequest<WarehouseDto>;
