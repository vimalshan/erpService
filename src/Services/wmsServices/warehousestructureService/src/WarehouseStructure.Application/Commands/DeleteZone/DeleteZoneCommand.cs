using MediatR;

namespace WarehouseStructure.Application.Commands.DeleteZone;

public sealed record DeleteZoneCommand(int Id) : IRequest<bool>;
