using MediatR;

namespace WarehouseStructure.Application.Commands.DeleteWarehouse;

public sealed record DeleteWarehouseCommand(int Id) : IRequest<bool>;
