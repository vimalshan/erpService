using MediatR;

namespace RackingSystem.Application.Features.Racks.Commands.DeleteRack;

public record DeleteRackCommand(int Id) : IRequest;
